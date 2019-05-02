"""
MMORTS main server program.
"""

import datetime
import json
import socket
import sys
import threading
import time
import traceback

import config
from core.core import Core


class Const:
    commands = [
        'ping',
        'help',
        'login',
        'spawn_unit',
        'move_unit',
        'map'
    ]


def now():
    return str(datetime.datetime.now())


def log(msg):
    print(f'[{now()}] {msg}')


def handle_request(addr, request, player_name, core):
    cmd = ''
    response = ''
    error = ''

    try:
        request = json.loads(request.decode('utf-8'))
        cmd = request.get('c', False)

        try:
            if cmd == 'ping':
                response = 'pong'

            elif cmd == 'help':
                response = Const.commands

            elif cmd == 'server_status':
                response = {'server': core.is_alive(), **core.threads_status()}

            elif cmd == 'login':
                player_name = request.get('player_name', False)
                core.add_player(player_name)
                response = player_name

            elif cmd == 'spawn_unit':
                unit_type_name = request.get('unit_type', False)
                error = core.spawn_unit(player_name, unit_type_name)

            elif cmd == 'move_unit':
                uid = int(request.get('uid', False))
                destination = request.get('destination', False)
                error = core.move_unit(player_name, uid, destination)

            elif cmd == 'map':
                response = core.get_map(player_name)

            else:
                error = f'Unknown command: {cmd}'

        except KeyError as e:
            error = f'Invalid command argument: {e}'

        except Exception as e:
            error = f'Error occured while processing {cmd} command: {e}'
            traceback.print_exc(file=sys.stderr)

    except Exception:
        error = f'Invalid request format'

    resp = {}
    if cmd:
        resp['c'] = cmd
    if response:
        resp['r'] = response
    if error:
        resp['e'] = error
        log(f'Client {addr} ({player_name}): {error}')

    return json.dumps(resp).encode('utf-8'), player_name


def authorisation(conn):
    #   TODO There should be a good authorization system
    request = conn.recv(1024)
    request = json.loads(request.decode('utf-8'))
    if request.get('c', False) == 'login':
        #   Get user from DB, verify that he is legit
        player_name = request.get('player_name', False)
        if player_name == 'DefaultPlayer':
            return player_name
    else:
        log('User must log in before using any other commands')
        return False


def client_connected(conn, addr, core):
    # TODO Create an object of a class 'User' whenever a connection occurs
    player_name = authorisation(conn)
    if player_name:
        conn.sendall(json.dumps({"r": "success"}).encode('utf-8'))
        log(f"Client logged in as {player_name}")
        while True:
            request = conn.recv(1024)
            if not request:
                break
            response, player_name = handle_request(addr, request, player_name, core)
            conn.sendall(response)
        log(f'Client {addr} ({player_name}) disconnected')
    else:
        conn.sendall(json.dumps({"r": "fail"}).encode('utf-8'))
        log("Something went wrong with client connection")


def run_server(core):
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.bind(config.SERVER_ADDRESS)
    sock.listen(1)
    log(f'Server started at {config.SERVER_ADDRESS}')
    while True:
        conn, addr = sock.accept()
        log(f'Client connected from address {addr}')
        thread = threading.Thread(
            target=client_connected,
            args=[conn, addr, core],
            daemon=True)
        thread.start()


def main():
    try:
        core = Core()
        main_thread = threading.Thread(
            target=run_server,
            args=[core],
            daemon=True)
        main_thread.start()
        while main_thread.isAlive() and core.is_alive():
            time.sleep(1)

    except KeyboardInterrupt:
        pass

    except Exception:
        with open('log.txt', 'a') as logfile_descriptor:
            print(now(), file=logfile_descriptor)
            traceback.print_exc(file=logfile_descriptor)
            print('\n', file=logfile_descriptor)


if __name__ == '__main__':
    main()
