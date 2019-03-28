"""
MMORTS main server program.
"""

import datetime
import json
import socket
import threading
import time
import traceback

import config


def now():
    return str(datetime.datetime.now())


def log(msg):
    print(f'[{now()}] {msg}\n')


def client_connected(conn, addr):
    while True:
        data = conn.recv(1024)
        if not data:
            break
        data = data.decode('utf-8')

        if data == 'ping':
            conn.sendall('pong'.encode('utf-8'))

        elif data == 'list':
            conn.sendall(json.dumps(
                [i for i in range(10)]).encode('utf-8'))

        elif data == 'dict':
            conn.sendall(json.dumps(
                {f'{i}': i for i in range(10)}).encode('utf-8'))

    log(f'Client {addr} disconnected')


def run_server():
    sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    sock.bind(config.SERVER_ADDRESS)
    sock.listen(1)
    log(f'Server started at {config.SERVER_ADDRESS}')
    while True:
        conn, addr = sock.accept()
        log(f'Client connected from address {addr}')
        thread = threading.Thread(
            target=client_connected,
            args=[conn, addr],
            daemon=True)
        thread.start()


def main():
    try:
        main_thread = threading.Thread(
            target=run_server,
            daemon=True)
        main_thread.start()
        while main_thread.isAlive():
            time.sleep(1)

    except KeyboardInterrupt:
        pass

    except Exception:
        with open('log.txt', 'a') as f:
            print(datetime.datetime.now(), file=f)
            traceback.print_exc(file=f)
            print('\n', file=f)


if __name__ == '__main__':
    main()
