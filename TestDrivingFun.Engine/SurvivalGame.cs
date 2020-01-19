using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace TestDrivingFun.Engine
{
    public class SurvivalGame
    {
        private readonly Random _rnd;
        private List<Event> _events;
        private Surface _surface;

        public SurvivalGame(Random rnd)
        {
            _rnd = rnd;
            _events = new List<Event>();
            _surface = new Surface(_events, _rnd);
        }

        public void CreateDefaultGame()
        {
            var createHerbivores = new List<Herbivore>();
            var createCarnivores = new List<Carnivore>();
            for (int i = 1; i < 11; i++)
            {
                createHerbivores.Add(new Herbivore(0, i, "h" + i));
                createCarnivores.Add(new Carnivore(19, i, "c" + i));
            }

            
            for (int i = 1; i < 11; i++)
            {
              
            }

            CreateNewGame(20, createHerbivores, createCarnivores);
        }

        public void CreateNewGame(int size, IEnumerable<Herbivore> herbivores, IEnumerable<Carnivore> carnivores)
        {
            _events = new List<Event>();
            var herbivoreList = herbivores.ToList();
            var carnivoreList = carnivores.ToList();
            _events.AddRange(new Surface(new List<Event>(), _rnd).Handle(new CreateBoard(size, size, herbivoreList, carnivoreList, "createBoard")));

            foreach (var herbivore in herbivoreList)
            {
                _surface = new Surface(_events, _rnd);
                _events.AddRange(_surface.Handle(new CreateHerbivore(herbivore, herbivore.Id, herbivore.Id)));
            }

            foreach (var carnivore in carnivoreList)
            {
                _surface = new Surface(_events, _rnd);
                _events.AddRange(_surface.Handle(new CreateCarnivore(carnivore, carnivore.Id, carnivore.Id)));
            }

            _surface = new Surface(_events, _rnd);
        }

        public void Bump()
        {
            var id = Guid.NewGuid().ToString();
            _events.AddRange(_surface.Handle(new BumpGame(id, id, id, DateTime.Now)));

            _surface = new Surface(_events, _rnd);


        }

        public int Size => _surface.Columns;
        public Surface.CellType[,] Cells => _surface.Cells;
    }

    public class BumpGame : Command
    {
        public BumpGame(string id, string causationId, string correlationId, DateTime createdOn) : base(id, causationId, correlationId, createdOn)
        {

        }
    }
}