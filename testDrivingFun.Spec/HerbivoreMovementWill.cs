using System;
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
    }
}