"""
Module describing game world.
"""

from random import uniform, randint
from sys import stderr
from threading import Event, Thread
from time import sleep
from traceback import print_exc

from .config import MINIMUM_TICK_DELAY, SECTOR_SIZE
from .map import Position, Space
from .unit import Unit


class World:
    def __init__(self):
        self.current_uid = 0
        self.map = [Space(space_id) for space_id in range(1)]
        self.units = {}
        self.threads = []
        self.event = Event()
        self.run_server = True

    def spawn_unit(self, player, unit_type):
        # not thread-safe: may break self.next_tick
        # TODO: make spawning units in self.next_tick
        sector = (randint(0, 9), randint(0, 9))
        pos = Position(
            0, sector, [uniform(0, SECTOR_SIZE), uniform(0, SECTOR_SIZE)])
        unit = Unit(self.current_uid, player, unit_type, pos)
        self.current_uid += 1
        self.units[unit.uid] = unit
        player.units[unit.uid] = unit
        self.map[0][sector][unit.uid] = unit

    def move_unit(self, player, uid, destination):
        self.units[uid].destination = Position.from_list(destination)

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
        for space in self.map:
            space.handle_movements()
