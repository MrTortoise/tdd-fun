using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace TestDrivingFun.Engine
{
    public class SurvivalGame
    {
        private List<Event> _events;
        private Surface _surface;

        public void CreateDefaultGame()
        {
            var createHerbivores = new List<Herbivore>()
            {
                new Herbivore(0,1,  string.Empty),
                new Herbivore(0,2, string.Empty),
                new Herbivore(0,3, string.Empty),
                new Herbivore(0,4, string.Empty),
                new Herbivore(0,5, string.Empty),
                new Herbivore(0,6, string.Empty),
                new Herbivore(0,7, string.Empty),
                new Herbivore(0,8, string.Empty),
                new Herbivore(0,9, string.Empty),
                new Herbivore(0,10, string.Empty)
            };
            var createCarnivores = new List<Carnivore>()
            {
                new Carnivore(19, 1, string.Empty),
                new Carnivore(19, 2, string.Empty),
                new Carnivore(19, 3, string.Empty),
                new Carnivore(19, 4, string.Empty),
                new Carnivore(19, 5, string.Empty),
                new Carnivore(19, 6, string.Empty),
                new Carnivore(19, 7, string.Empty),
                new Carnivore(19, 8, string.Empty),
                new Carnivore(19, 9, string.Empty),
                new Carnivore(19, 10, string.Empty)
            };
            CreateNewGame(20, createHerbivores, createCarnivores);
        }

        public void CreateNewGame(int size, IEnumerable<Herbivore> herbivores, IEnumerable<Carnivore> carnivores)
        {
            _events = new List<Event>();
            var herbivoreList = herbivores.ToList();
            var carnivoreList = carnivores.ToList();
            _events.AddRange(new Surface(new List<Event>()).Handle(new CreateBoard(size, size, herbivoreList, carnivoreList, "createBoard")));

            foreach (var herbivore in herbivoreList)
            {
                _surface = new Surface(_events);
                _events.AddRange(_surface.Handle(new CreateHerbivore(herbivore, herbivore.Id, herbivore.Id)));
            }

            foreach (var carnivore in carnivoreList)
            {
                var _surface = new Surface(_events);
                _events.AddRange(_surface.Handle(new CreateCarnivore(carnivore, carnivore.Id, carnivore.Id)));
            }

            _surface = new Surface(_events);
        }

        public int Size => _surface.Columns;
        public Surface.CellType[,] Cells => _surface.Cells;
    }
}