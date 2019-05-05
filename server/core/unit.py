"""
Module describing game units.
"""


class UnitType:
    def __init__(self, name, player, params):
        self.name = name
        self.player = player
        self.params = params


class Unit:
    def __init__(self, uid, player, unit_type, position):
        self.uid = uid
        self.type = unit_type
        self.player = player
        self.position = position
        self.destination = None

    def to_dict(self, private=None):
        result = {
            'uid': str(self.uid),
            'type': self.type.name,
            'position': self.position.to_list(),
        }
        if private == self.player:
            if self.destination:
                result.update({
                    'destination': self.destination.to_list()
                })
        return result
