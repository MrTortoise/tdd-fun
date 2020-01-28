using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestDrivingFun.Engine;
using Xunit;

namespace testDrivingFun.Spec
{
    public class CreateSurvivalGameWill
    {
        private readonly SurvivalGame _ut;
        private readonly MockInMemoryEventStore _eventStore;

        public CreateSurvivalGameWill()
        {
            _eventStore = new MockInMemoryEventStore();
            _ut = new SurvivalGame(nameof(CreateSurvivalGameWill), new Random(), _eventStore);
            var createHerbivores = new List<Herbivore>();
            for (int i = 1; i <= 10; i++)
            {
                createHerbivores.Add(new Herbivore(0, i, "h" + i));
            }

            var createCarnivores = new List<Carnivore>();
            for (int i = 1; i <= 10; i++)
            {
                createCarnivores.Add(new Carnivore(19, i, "c" + i, 10));
            }

            _ut.CreateNewGame(20, createHerbivores, createCarnivores);
        }

        [Fact]
        public void HaveABoardThatIsRightSize()
        {
         
            Assert.Equal(20, _ut.Cells.GetLength(0));
            Assert.Equal(20, _ut.Cells.GetLength(1));
        }

        [Fact]
        public void HaveTheRightNumberOfHerbivores()
        {
            var eventStream = _eventStore.Events.Values.First();
            Assert.Equal(10, eventStream.Count(e=>e.GetType().Name == typeof(CreateHerbivoreAccepted).Name));
        }

        [Fact]
        public void HaveTheRightNumberOfCarnivores()
        {
            var eventStream = _eventStore.Events.Values.First();
            Assert.Equal(10, eventStream.Count(e => e.GetType().Name == typeof(CreateCarnivoreAccepted).Name));
        }
    }
}
