import json
import socket

import config


def make_request(c, **kwargs):
    return {'c': c, **kwargs}


def make_response(c=None, r=None, e=None):
    result = {}
    for x, y in [('c', c), ('r', r), ('e', e)]:
        if y:
            result[x] = y
    return result


class Test_Connection:
    def connect(self):
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        sock.connect(config.SERVER_ADDRESS)
        return sock

    def send_and_recv(self, sock, data):
        sock.sendall(json.dumps(data).encode('utf-8'))
        response = json.loads(sock.recv(1024 * 1024).decode('utf-8'))
        return response

    def login(self, sock, player_name):
        return self.send_and_recv(sock, make_request(
            'login', player_name=player_name))

    def spawn_unit(self, sock, unit_type):
        return self.send_and_recv(sock, make_request(
            'spawn_unit', unit_type=unit_type))

    def move_unit(self, sock, uid, destination):
        return self.send_and_recv(sock, make_request(
            'move_unit', uid=uid, destination=destination))

    def map(self, sock):
        return self.send_and_recv(sock, make_request('map'))

    def test_ping(self):
        with self.connect() as sock:
            response = self.send_and_recv(sock, make_request('ping'))
        assert response == make_response('ping', 'pong')

    def test_abrakadabra(self):
        with self.connect() as sock:
            response = self.send_and_recv(sock, 'something')
        assert response == make_response(e='Invalid request format')

    def test_login(self):
        with self.connect() as sock:
            response = self.login(sock, 'PlayerName73')
        assert response == make_response('login', 'PlayerName73')

    def test_empty_player_name(self):
        with self.connect() as sock:
            response = self.spawn_unit(sock, '')
        assert response == make_response(
            'spawn_unit', e='Player name cannot be empty')

    def test_invalid_unit_type(self):
        with self.connect() as sock:
            self.login(sock, 'PlayerName73')
            response = self.spawn_unit(sock, 'invalid')
        assert response == make_response(
            'spawn_unit', e='Unit type invalid does not exist')

    def test_move_nonexistent_unit(self):
        with self.connect() as sock:
            self.login(sock, 'PlayerName73')
            response = self.move_unit(sock, '-1', [0, 0, 0])
        assert response == make_response(
            'move_unit', e='No unit with uid -1')

    def test_move_foreign_unit(self):
        with self.connect() as sock:
            self.login(sock, 'PlayerName73')
            self.spawn_unit(sock, 'basic_circle')
            self.login(sock, 'WrongPlayer')
            response = self.move_unit(sock, 0, [0, 0, 0])
        assert response == make_response(
            'move_unit', e='Unit 0 does not belong to player WrongPlayer')

    def test_move_unit(self):
        with self.connect() as sock:
            self.login(sock, 'PlayerName73')
            self.spawn_unit(sock, 'basic_circle')
            response = self.move_unit(sock, 0, [0, 0, 0])
        assert response == make_response('move_unit')

    def test_map_no_errors(self):
        with self.connect() as sock:
            response = self.map(sock)
            print(response)
            assert response.get('e', 'Banana') == 'Banana'
