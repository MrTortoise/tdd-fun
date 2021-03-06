﻿using System;
using System.Collections.Generic;
using System.Linq;
using TestDrivingFun.Engine;
using Xunit;

namespace testDrivingFun.Spec
{
    public class HerbivoreMovementWill
    {
        private MockInMemoryEventStore _eventStore;
        private SurvivalGame _ut;

        [Fact]
        public void OnlyMoveOneSquareAway()
        {
            _eventStore = new MockInMemoryEventStore();
            _ut = new SurvivalGame(nameof(CreateSurvivalGameWill), new Random(), _eventStore);
            var herbivores = new List<Herbivore>() { new Herbivore(10, 10, "h1") };
            _ut.CreateNewGame(20, herbivores, new List<Carnivore> ());

            _ut.Bump();

            var @event = _eventStore.Events.Values.First().Last();
            Assert.Equal(typeof(HerbivoreMoved).Name, @event.GetType().Name);

            var movedEvent = (HerbivoreMoved)@event;
            Assert.Equal(10, movedEvent.OldPosition.X);
            Assert.Equal(10, movedEvent.OldPosition.Y);

            Assert.True(movedEvent.NewPosition.X >= 9);
            Assert.True(movedEvent.NewPosition.X <= 11);

            Assert.True(movedEvent.NewPosition.Y >= 9);
            Assert.True(movedEvent.NewPosition.Y <= 11);

            Assert.False(movedEvent.OldPosition.X == movedEvent.NewPosition.X && movedEvent.OldPosition.Y == movedEvent.NewPosition.Y);
        }

        [Fact]
        public void AdjacentHerbivoresLayEggs()
        {
            _eventStore = new MockInMemoryEventStore();
            _ut = new SurvivalGame(nameof(CreateSurvivalGameWill), new Random(), _eventStore);
            var herbivores = new List<Herbivore>()
            {
                new Herbivore(10, 10, "h1"),
                new Herbivore(10, 11, "h2")
            };
            _ut.CreateNewGame(20, herbivores, new List<Carnivore>());

            _ut.Bump();

            var eventStream = _eventStore.Events.Values.First();
            var herbivoreLaidEggsEvents = eventStream.Where(e=>e.GetType().Name == typeof(HerbivoreLaidEgg).Name).ToList();
            Assert.True(herbivoreLaidEggsEvents.Any());

            var laidEggEvent = (HerbivoreLaidEgg)herbivoreLaidEggsEvents.First();
            Assert.Equal(10, laidEggEvent.X);
            Assert.Equal(10, laidEggEvent.Y);


            //var movedEvent = (HerbivoreMoved)@event;
            //Assert.Equal(10, movedEvent.OldPosition.X);
            //Assert.Equal(10, movedEvent.OldPosition.Y);

            //Assert.True(movedEvent.NewPosition.X >= 9);
            //Assert.True(movedEvent.NewPosition.X <= 11);

            //Assert.True(movedEvent.NewPosition.Y >= 9);
            //Assert.True(movedEvent.NewPosition.Y <= 11);

            //Assert.False(movedEvent.OldPosition.X == movedEvent.NewPosition.X && movedEvent.OldPosition.Y == movedEvent.NewPosition.Y);

        }
    }
}