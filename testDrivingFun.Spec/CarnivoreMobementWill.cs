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

            var movedEvent = (CarnivoreMoved)@event;
            Assert.Equal(10, movedEvent.OldPosition.X);
            Assert.Equal(10, movedEvent.OldPosition.Y);

            Assert.True(movedEvent.NewPosition.X >= 9);
            Assert.True(movedEvent.NewPosition.X <= 11);

            Assert.True(movedEvent.NewPosition.Y >= 9);
            Assert.True(movedEvent.NewPosition.Y <= 11);

            Assert.False(movedEvent.OldPosition.X == movedEvent.NewPosition.X && movedEvent.OldPosition.Y == movedEvent.NewPosition.Y);
        }

        [Fact]
        public void EatHerbivoreIfNextToIt()
        {
            _eventStore = new MockInMemoryEventStore();
            var carnivoreId = "c1";
            var herbivoreId = "h1";
            _ut = new SurvivalGame(nameof(CreateSurvivalGameWill), new Random(), _eventStore);
            var createCarnivore = new List<Carnivore> { new Carnivore(10, 10, carnivoreId) };
            var createHerbivore = new List<Herbivore> { new Herbivore(10, 9, herbivoreId) };
            _ut.CreateNewGame(20, createHerbivore, createCarnivore);

            _ut.Bump();

            var @event = _eventStore.Events.Values.First().Last();
            Assert.Equal(typeof(CarnivoreAteHerbivore).Name, @event.GetType().Name);
            var carnivoreAteHerbivore = (CarnivoreAteHerbivore)@event;
            Assert.Equal(carnivoreId, carnivoreAteHerbivore.CarnivoreId);

            Assert.Equal(Surface.CellType.Default,
                _ut.Cells[carnivoreAteHerbivore.OldPosition.X, carnivoreAteHerbivore.OldPosition.Y]);
            Assert.Equal(Surface.CellType.Carnivore,
                _ut.Cells[carnivoreAteHerbivore.NewPosition.X, carnivoreAteHerbivore.NewPosition.Y]);
        }

        [Fact]
        public void CarnivoresMoveApartFromEachOther()
        {
            _eventStore = new MockInMemoryEventStore();
            const string C1 = "c1";
            const string C2 = "C2";
            _ut = new SurvivalGame(nameof(CreateSurvivalGameWill), new Random(), _eventStore);
            var createCarnivore = new List<Carnivore>
                {
                    new Carnivore(10, 10, C1),
                    new Carnivore(10, 9, C2)
                };
            _ut.CreateNewGame(20, new List<Herbivore>(), createCarnivore);

            _ut.Bump();

            var events = _eventStore.Events.Values.First();
            var last2Events = events.Skip(events.Count - 2).ToList();
            var last = last2Events.Last();
            var secondFromLast = last2Events.First();

            Assert.Equal(typeof(CarnivoreMoved).Name, last.GetType().Name);
            Assert.Equal(typeof(CarnivoreMoved).Name, secondFromLast.GetType().Name);

            var e = (CarnivoreMoved) secondFromLast;
            Assert.Equal(10, e.NewPosition.X);
            Assert.Equal(11, e.NewPosition.Y);
        }

        [Fact]
        public void CarnivoresMoveApartFromEachOther_UnlessMoveInvalid()
        {
            _eventStore = new MockInMemoryEventStore();
            const string C1 = "c1";
            const string C2 = "C2";
            _ut = new SurvivalGame(nameof(CreateSurvivalGameWill), new Random(), _eventStore);
            var createCarnivore = new List<Carnivore>
            {
                new Carnivore(0, 0, C1),
                new Carnivore(0, 1, C2)
            };
            _ut.CreateNewGame(20, new List<Herbivore>(), createCarnivore);

            _ut.Bump();

            var events = _eventStore.Events.Values.First();
            var last2Events = events.Skip(events.Count - 2).ToList();
            var last = last2Events.Last();
            var secondFromLast = last2Events.First();

            Assert.Equal(typeof(CarnivoreMoved).Name, last.GetType().Name);
            Assert.Equal(typeof(CarnivoreMoved).Name, secondFromLast.GetType().Name);

            var e = (CarnivoreMoved)secondFromLast;
            Assert.Equal(1, e.NewPosition.X); // x cannot stay 0 during any move
            Assert.InRange(e.NewPosition.Y,0, 1); // 1,0 and 1,1 are valid
        }
    }
}
