"""
MMORTS client module.
"""

import json
import socket
import sys
import traceback

import config


class ClientAPI:
    def __init__(self):
        self.sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        self.sock.connect(config.SERVER_ADDRESS)

    def __enter__(self):
        return self

    def __exit__(self, exc_type, exc_value, traceback):
        self.sock.close()

    def _send(self, data):
        self.sock.sendall(json.dumps(data).encode('utf-8'))

    def _recv(self):
        return json.loads(self.sock.recv(1024 * 1024).decode('utf-8'))

    def _make_request(self, cmd, **kwargs):
        return {'c': cmd, **kwargs}

    def execute_command(self, cmd, **kwargs):
        self._send(self._make_request(cmd, **kwargs))
        return self._recv()

    def login(self, player_name):
        return self.execute_command('login', player_name=player_name)

    def spawn_unit(self, unit_type):
        return self.execute_command('spawn_unit', unit_type=unit_type)

    def move_unit(self, uid, destination):
        return self.execute_command('move_unit', uid=uid, destination=destination)

    def map(self):
        return self.execute_command('map')


def main():
    api = ClientAPI()

    commands = {
        'login': ['player_name'],
        'spawn_unit': ['unit_type'],
        'move_unit': ['uid', 'destination'],
        'map': [],
    }

    while True:
        raw_request = input('>>> ')
        req = raw_request.split(' ')

        try:
            cmd, args = req[0], req[1:]
            if cmd in commands.keys():
                kwargs = {x: y for x, y in zip(commands[cmd], args)}

                if cmd == 'move_unit':
                    kwargs['destination'] = json.loads(kwargs['destination'])

            else:
                kwargs = {x.split('=')[0]: x.split('=')[1] for x in args}

            request = api._make_request(cmd, **kwargs)
            response = api.execute_command(cmd, **kwargs)

            print(f'\nrequest: {request}\n\nresponse: {response}\n\n')

        except KeyboardInterrupt:
            break

        except Exception:
            print(f'Error occured while handling this input: {req}\n')
            traceback.print_exc(file=sys.stdout)
            print(f'\n')


if __name__ == '__main__':
    main()
