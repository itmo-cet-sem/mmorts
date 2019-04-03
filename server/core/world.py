"""
Module describing game world.
"""

from math import sqrt
from random import uniform
from sys import stderr
from threading import Event, Thread
from time import sleep
from traceback import print_exc

from .config import MINIMUM_TICK_DELAY
from .unit import Unit


class Position:
    def __init__(self, space, x, y):
        self.space, self.x, self.y = space, x, y

    def to_list(self):
        return [self.space, self.x, self.y]

    def __add__(self, other):
        if isinstance(other, Position):
            return Position(self.space, self.x + other.x, self.y + other.y)
        return Position(self.space, self.x + other[0], self.y + other[1])

    def __sub__(self, other):
        if isinstance(other, Position):
            return Position(self.space, self.x - other.x, self.y - other.y)
        return Position(self.space, self.x - other[0], self.y - other[1])

    def __abs__(self):
        return sqrt(self.x ** 2 + self.y ** 2)

    def norm(self):
        mod = max(1, abs(self))
        return Position(self.space, self.x / mod, self.y / mod)


class World:
    def __init__(self):
        self.current_uid = 0
        self.units = {}
        self.threads = []
        self.event = Event()
        self.run_server = True

    def spawn_unit(self, player, unit_type):
        # not thread-safe: may break self.next_tick
        # TODO: make spawning units in self.next_tick
        unit = Unit(
            self.current_uid, player, unit_type,
            Position(0, uniform(-100, 100), uniform(-100, 100)))
        self.current_uid += 1
        self.units[unit.uid] = unit
        player.units[unit.uid] = unit

    def move_unit(self, player, uid, destination):
        self.units[uid].destination = Position(*destination)

    def start(self):
        self.threads.append(Thread(
            name='timer',
            target=self.timer,
            daemon=True))
        self.threads.append(Thread(
            name='wait_timer',
            target=self.wait_timer,
            daemon=True))
        for x in self.threads:
            x.start()

    def timer(self):
        skipped = 0
        while self.run_server:
            if self.event.is_set():
                skipped += 1
                print(f'Skipped {skipped} ticks')
            else:
                skipped = 0
            self.event.set()
            sleep(MINIMUM_TICK_DELAY)

    def wait_timer(self):
        try:
            while self.run_server:
                if not self.event.wait(MINIMUM_TICK_DELAY):
                    continue
                self.next_tick()
                self.event.clear()
        except Exception:
            self.run_server = False
            print_exc(file=stderr)

    def next_tick(self):
        for unit in self.units.values():
            if unit.destination is None:
                continue
            direction = (unit.destination - unit.position).norm()
            unit.position += direction
            if abs(direction) < 1:
                unit.destination = None
