from math import sqrt

from .config import SECTOR_SIZE


class Vector:
    def __init__(self, x, y):
        self.x, self.y = x, y

    def __str__(self):
        return f'({self.x} {self.y})'

    def __repr__(self):
        return f'Vector({self.x}, {self.y})'

    def __eq__(self, other):
        return self.x == other[0] and self.y == other[1]

    def __add__(self, other):
        return Vector(self.x + other[0], self.y + other[1])

    def __sub__(self, other):
        return Vector(self.x - other[0], self.y - other[1])

    def __abs__(self):
        return sqrt(self.x ** 2 + self.y ** 2)

    def __getitem__(self, key):
        if key == 0:
            return self.x
        elif key == 1:
            return self.y
        else:
            raise IndexError(f'Invalid key ({key}) for {type(self)} object')

    def __setitem__(self, key, value):
        if key == 0:
            self.x = value
        elif key == 1:
            self.y = value
        else:
            raise IndexError(f'Invalid key ({key}) for {type(self)} object')

    def norm(self):
        mod = max(1, abs(self))
        return Vector(self.x / mod, self.y / mod)


class Position:
    def __init__(self, space, sector, pos):
        self.space = space
        self.sector = Vector(*sector)
        self.pos = Vector(*pos)

    def to_list(self):
        return [self.space, *self.sector, *self.pos]

    @staticmethod
    def from_list(lst):
        return Position(lst[0], lst[1:3], lst[3:])

    def __add__(self, other):
        new_sector = Vector(*self.sector)
        new_pos = self.pos + other
        for coord in [0, 1]:
            if not 0 <= new_pos[coord] < SECTOR_SIZE:
                new_sector[coord] += round(new_pos[coord] // SECTOR_SIZE)
                new_pos[coord] %= SECTOR_SIZE
        return Position(self.space, new_sector, new_pos)

    def __sub__(self, other):
        sx = self.sector.x - other.sector.x
        sy = self.sector.y - other.sector.y
        px = self.pos.x - other.pos.x
        py = self.pos.y - other.pos.y
        return Vector(sx * SECTOR_SIZE + px, sy * SECTOR_SIZE + py)


class Space:
    def __init__(self, space_id):
        self.id = space_id
        self.sectors = {}

    def handle_movements(self):
        for sector in self.sectors.values():
            for unit in sector.values():
                if unit.destination is None:
                    continue
                direction = (unit.destination - unit.position).norm()
                unit.position += direction
                if abs(direction) < 1e-4:
                    unit.destination = None
        to_move = []
        for sector_id, sector in self.sectors.items():
            to_remove = []
            for uid, unit in sector.items():
                if unit.position.sector != sector_id:
                    to_remove.append(uid)
                    to_move.append(unit)
            for uid in to_remove:
                sector.pop(uid)
        for unit in to_move:
            self[unit.position.sector][unit.uid] = unit

    def __getitem__(self, key):
        key = tuple(key)
        if key not in self.sectors:
            self.sectors[key] = {}
        return self.sectors[key]

    def __setitem__(self, key, value):
        key = tuple(key)
        if key not in self.sectors:
            self.sectors[key] = {}
        self.sectors[key] = value
