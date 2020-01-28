using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestDrivingFun.Engine;
using Xunit;

namespace testDrivingFun.Spec
{
    public class CarnivoreLifeCycleWill
    {
        private MockInMemoryEventStore _eventStore;
        private SurvivalGame _ut;

        [Fact]
        public void WillDieAfter10Moves()
        {
            _eventStore = new MockInMemoryEventStore();
            _ut = new SurvivalGame(nameof(CreateSurvivalGameWill), new Random(), _eventStore);
            var createCarnivores = new List<Carnivore> { new Carnivore(10, 10, "c1", 10) };
            _ut.CreateNewGame(20, new List<Herbivore>(), createCarnivores);

            for (int i = 0; i < 11; i++)
            {
                _ut.Bump();
            }
            
            var @event = _eventStore.Events.Values.First().Last();
            Assert.Equal(typeof(CarnivoreDied).Name, @event.GetType().Name);
        }
    }
}
