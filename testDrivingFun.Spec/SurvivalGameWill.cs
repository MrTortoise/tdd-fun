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
        private SurvivalGame _ut;
        private List<Herbivore> _createHerbivores;
        private InMemoryEventStore _eventStore;

        public CreateSurvivalGameWill()
        {
            _eventStore = new InMemoryEventStore();
            _ut = new SurvivalGame(nameof(CreateSurvivalGameWill), new Random(), _eventStore);
            _createHerbivores = new List<Herbivore>();
            for (int i = 1; i <= 10; i++)
            {
                _createHerbivores.Add(new Herbivore(0, i, "h" + i));
            }

            var createCarnivores = new List<Carnivore>();
            for (int i = 1; i <= 10; i++)
            {
                createCarnivores.Add(new Carnivore(19, i, "c" + i));
            }

            _ut.CreateNewGame(20, _createHerbivores, createCarnivores);
        }

        [Fact]
        public void HaveABoardThatIsRightSize()
        {
         
            Assert.Equal(20, _ut.Cells.GetLength(0));
            Assert.Equal(20, _ut.Cells.GetLength(1));
        }

        [Fact]
        public void HAveTheRightNumberOfHerbivores()
        {
            
        }
    }
}
