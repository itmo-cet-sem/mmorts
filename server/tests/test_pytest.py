from client import ClientAPI


def make_response(c=None, r=None, e=None):
    result = {}
    for x, y in [('c', c), ('r', r), ('e', e)]:
        if y:
            result[x] = y
    return result


class Test_Connection:
    def test_ping(self):
        response = ClientAPI().execute_command('ping')
        assert response == make_response('ping', 'pong')

    def test_abrakadabra(self):
        with ClientAPI() as cl:
            cl._send('something')
            response = cl._recv()
        assert response == make_response(e='Invalid request format')

    def test_login(self):
        response = ClientAPI().login('TestPlayer0')
        assert response == make_response('login', 'TestPlayer0')

    def test_empty_player_name(self):
        with ClientAPI() as cl:
            response = cl.spawn_unit('')
        assert response == make_response(
            'spawn_unit', e='Player name cannot be empty')

    def test_invalid_unit_type(self):
        with ClientAPI() as cl:
            cl.login('TestPlayer2')
            response = cl.spawn_unit('invalid')
        assert response == make_response(
            'spawn_unit', e='Unit type invalid does not exist')

    def test_unit_type(self):
        with ClientAPI() as cl:
            cl.login('TestPlayer3')

            response = cl.register_unit_type('test_unit_type', {'radius': 1})
            assert response == make_response('register_unit_type')

            response = cl.get_unit_types()
            assert response == make_response(
                'get_unit_types', [{
                    'name': 'test_unit_type',
                    'params': {'radius': 1}}])

            response = cl.delete_unit_type('test_unit_type')
            assert response == make_response('delete_unit_type')

            response = cl.get_unit_types()
            assert response == make_response('get_unit_types', [])

    def test_register_duplicate_unit_type(self):
        with ClientAPI() as cl:
            cl.login('TestPlayer4')
            cl.register_unit_type('dup_unit_type', {})
            response = cl.register_unit_type('dup_unit_type', {})
        assert response == make_response(
            'register_unit_type', e='Unit type dup_unit_type already exists')

    def test_move_nonexistent_unit(self):
        with ClientAPI() as cl:
            cl.login('TestPlayer5')
            response = cl.move_unit('-1', [0, 0, 0])
        assert response == make_response(
            'move_unit', e='No unit with uid -1')

    def test_move_foreign_unit(self):
        with ClientAPI() as cl:
            cl.login('TestPlayer6')
            cl.register_unit_type('test_unit_type', {})
            cl.spawn_unit('test_unit_type')
            cl.login('TestPlayer7')
            response = cl.move_unit(0, [0, 0, 0])
        assert response == make_response(
            'move_unit', e='Unit 0 does not belong to player TestPlayer7')

    def test_map_no_errors(self):
        with ClientAPI() as cl:
            response = cl.map()
            assert response.get('e', 'Banana') == 'Banana'

    def test_move_unit(self):
        with ClientAPI() as cl:
            cl.login('TestPlayer8')
            cl.register_unit_type('test_unit_type', {})
            cl.spawn_unit('test_unit_type')
            response = cl.move_unit(1, [0, 0, 0])
        assert response == make_response('move_unit')
