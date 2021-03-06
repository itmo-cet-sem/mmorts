"""
The game core module.
"""

from .player import Player
from .unit import UnitType
from .world import World


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
    def register_unit_type(self, player, unit_type_name, params):
        if unit_type_name in player.unit_types:
            return f'Unit type {unit_type_name} already exists'
        player.unit_types[unit_type_name] = UnitType(
            unit_type_name, player, params)

    @check_player
    def delete_unit_type(self, player, unit_type_name):
        if unit_type_name not in player.unit_types:
            return
        for unit in player.units.values():
            if unit.type.name == unit_type_name:
                return (f'Cannot delete unit type {unit_type_name} '
                        f'if there are units of this type exist')
        player.unit_types.pop(unit_type_name)

    @check_player
    def get_unit_types(self, player):
        return [{'name': x, 'params': y.params}
                for x, y in player.unit_types.items()]

    @check_player
    def spawn_unit(self, player, unit_type_name):
        if unit_type_name not in player.unit_types:
            return f'Unit type {unit_type_name} does not exist'
        self.world.spawn_unit(
            player, player.unit_types[unit_type_name])

    @check_player
    def move_unit(self, player, uid, destination):
        if uid not in self.world.units:
            return f'No unit with uid {uid}'
        if uid not in player.units:
            return f'Unit {uid} does not belong to player {player.name}'
        self.world.move_unit(player, uid, destination)

    @check_player
    def get_map(self, player, space, sectors):
        result = []
        for sector in sectors:
            units = [x.to_dict(player) for x in
                     self.world.map[space][sector].values()]
            result.append({'space': space, 'sector': sector, 'units': units})
        return result
