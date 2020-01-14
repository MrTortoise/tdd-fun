using System.Collections.Generic;
using System.Linq;

namespace TestDrivingFun.Engine
{
    public class SurvivalGame
    {
        private List<Event> _events;

        public void CreateNewGame(int size, IEnumerable<Herbivore> herbivores, IEnumerable<Carnivore> carnivores)
        {
            _events = new List<Event>();
            var herbivoreList = herbivores.ToList();
            var carnivoreList = carnivores.ToList();
            _events.AddRange(new Surface(new List<Event>()).Handle(new CreateBoard(size, size, herbivoreList, carnivoreList, "createBoard")));

            foreach (var herbivore in herbivoreList)
            {
                var surface = new Surface(_events);
                _events.AddRange(surface.Handle(new CreateHerbivore(herbivore, herbivore.Id, herbivore.Id)));
            }

            foreach (var carnivore in carnivoreList)
            {
                var surface = new Surface(_events);
                _events.AddRange(surface.Handle(new CreateCarnivore(carnivore, carnivore.Id, carnivore.Id)));
            }

        }
    }
}