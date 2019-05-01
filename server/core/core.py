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


def check_player(func):
    def wrapper(self, player_name, *args, **kwargs):
        if player_name == '':
            return f'Player name cannot be empty'
        if player_name not in self.players:
            return f'Player {player_name} does not exist'
        return func(self, self.players[player_name], *args, **kwargs)
    return wrapper


class Core:
    def __init__(self):
        self.players = {}
        self.world = World()
        self.world.start()

    def is_alive(self):
        return self.world.run_server

    def threads_status(self):
        return {x.name: x.is_alive() for x in self.world.threads}

    def add_player(self, player_name):
        if player_name not in self.players:
            self.players[player_name] = Player(player_name)
            return f'Player {player_name} successfully created'
        else:
            return f'Player {player_name} already exists'

    @check_player
    def spawn_unit(self, player, unit_type_name):
        if unit_type_name not in UNIT_TYPES:
            return f'Unit type {unit_type_name} does not exist'
        self.world.spawn_unit(
            player, UNIT_TYPES[unit_type_name])

    @check_player
    def move_unit(self, player, uid, destination):
        if uid not in self.world.units:
            return f'No unit with uid {uid}'
        if uid not in player.units:
            return f'Unit {uid} does not belong to player {player.name}'
        self.world.move_unit(player, uid, destination)

    def get_map(self, player_name):
        result = {
            'player_names': list(self.players.keys()),
            'players': []
        }
        for player in self.players.values():
            is_private = player.name == player_name
            units = {
                x.uid: x.to_dict(private=is_private)
                for x in player.units.values()
            }
            result['players'].append(units)
        return result
