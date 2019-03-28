import json
import socket

import config


class Test_Connection:
    def connect(self):
        sock = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
        sock.connect(config.SERVER_ADDRESS)
        return sock

    def test_ping(self):
        with self.connect() as sock:
            sock.sendall('ping'.encode('utf-8'))
            data = sock.recv(1024).decode('utf-8')
            assert data == 'pong'

    def test_list(self):
        with self.connect() as sock:
            sock.sendall('list'.encode('utf-8'))
            data = json.loads(sock.recv(1024).decode('utf-8'))
            assert data == [i for i in range(10)]

    def test_dict(self):
        with self.connect() as sock:
            sock.sendall('dict'.encode('utf-8'))
            data = json.loads(sock.recv(1024).decode('utf-8'))
            assert data == {f'{i}': i for i in range(10)}
