using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestDrivingFun.Engine;
using Xunit;

namespace testDrivingFun.Spec
{
    public class CarnivoreMovementWill
    {
        private MockInMemoryEventStore _eventStore;
        private SurvivalGame _ut;

        [Fact]
        public void OnlyMoveOneSquareAway()
        {
            _eventStore = new MockInMemoryEventStore();
            _ut = new SurvivalGame(nameof(CreateSurvivalGameWill), new Random(), _eventStore);
            var createCarnivores = new List<Carnivore> { new Carnivore(10, 10, "c1") };
            _ut.CreateNewGame(20, new List<Herbivore>(), createCarnivores);

            _ut.Bump();
            var @event = _eventStore.Events.Values.First().Last();
            Assert.Equal(typeof(CarnivoreMoved).Name, @event.GetType().Name);

            var movedEvent = (CarnivoreMoved) @event;
            Assert.Equal(10,movedEvent.OldPosition.X);
            Assert.Equal(10, movedEvent.OldPosition.Y);

            Assert.True(movedEvent.NewPosition.X >= 9);
            Assert.True(movedEvent.NewPosition.X <= 11);

            Assert.True(movedEvent.NewPosition.Y >= 9);
            Assert.True(movedEvent.NewPosition.Y <= 11);

            Assert.False(movedEvent.OldPosition.X == movedEvent.NewPosition.X && movedEvent.OldPosition.Y == movedEvent.NewPosition.Y);
        }
    }
}
