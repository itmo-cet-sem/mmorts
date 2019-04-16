"""
The game core module.
"""

from math import sqrt

from .player import Player
from .unit import UnitType
from .world import World


UNIT_TYPES = {
    'basic_circle': UnitType('basic_circle', 1),
    'basic_square': UnitType('basic_square', sqrt(2)),
}


class Core:
    def __init__(self):
        self.players = {}
        self.world = World()
        self.world.start()

    def is_alive(self):
        return self.world.run_server

    def threads_status(self):
        return {x.name: x.is_alive() for x in self.world.threads}

    def check_player(self, player_name):
        if player_name == '':
            return f'Player name cannot be empty'
        if player_name not in self.players:
            return f'Player {player_name} does not exist'
        return None

    def add_player(self, player_name):
        if player_name not in self.players:
            self.players[player_name] = Player(player_name)
            return f'Player {player_name} successfully created'
        else:
            return f'Player {player_name} already exists'

    def spawn_unit(self, player_name, unit_type_name):
        player_check = self.check_player(player_name)
        if player_check:
            return player_check
        if unit_type_name not in UNIT_TYPES:
            return f'Unit type {unit_type_name} does not exist'
        self.world.spawn_unit(
            self.players[player_name], UNIT_TYPES[unit_type_name])

    def move_unit(self, player_name, uid, destination):
        player_check = self.check_player(player_name)
        if player_check:
            return player_check
        player = self.players[player_name]
        if uid not in self.world.units:
            return f'No unit with uid {uid}'
        if uid not in player.units:
            return f'Unit {uid} does not belong to player {player_name}'
        self.world.move_unit(player, uid, destination)

    def get_map(self, player_name):
        result = {
            'player_names': list(self.players.keys()),
            'players': []
        }
        for player in self.players.values():
            units = {x.uid: x.to_dict() for x in player.units.values()}
            result['players'].append(units)
        return result
