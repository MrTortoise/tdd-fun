using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;

namespace TestDrivingFun.Engine
{
    public class SurvivalGame
    {
        private readonly string _gameId;
        private readonly Random _rnd;
        private readonly IEventStore _eventStore;
        private Surface _surface;

        public string GetStreamId()
        {
            return "game-" + _gameId;
        }

        public SurvivalGame(string gameId, Random rnd, IEventStore eventStore)
        {
            _gameId = gameId;
            _rnd = rnd;
            _eventStore = eventStore;
            _surface = new Surface(_eventStore.ReadStream(GetStreamId()), _rnd);
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


            CreateNewGame(20, createHerbivores, createCarnivores);
        }

        public void CreateNewGame(int size, IEnumerable<Herbivore> herbivores, IEnumerable<Carnivore> carnivores)
        {
            var herbivoreList = herbivores.ToList();
            var carnivoreList = carnivores.ToList();
            
            var events  = new Surface(_eventStore.ReadStream(GetStreamId()), _rnd)
                .Handle(new CreateBoard(size, size, herbivoreList, carnivoreList, "createBoard"));

            _eventStore.WriteEVents(GetStreamId(), events);

            foreach (var herbivore in herbivoreList)
            {
                _surface = new Surface(_eventStore.ReadStream(GetStreamId()), _rnd);
                _eventStore.WriteEVents(GetStreamId(), _surface.Handle(new CreateHerbivore(herbivore, herbivore.Id, herbivore.Id)));
            }

            foreach (var carnivore in carnivoreList)
            {
                _surface = new Surface(_eventStore.ReadStream(GetStreamId()), _rnd);
                _eventStore.WriteEVents(GetStreamId(), _surface.Handle(new CreateCarnivore(carnivore, carnivore.Id, carnivore.Id)));
            }

            _surface = new Surface(_eventStore.ReadStream(GetStreamId()), _rnd);
        }

        public void Bump()
        {
            var id = Guid.NewGuid().ToString();
            _eventStore.WriteEVents(GetStreamId(), _surface.Handle(new BumpGame(id, id, id, DateTime.Now)));

            _surface = new Surface(_eventStore.ReadStream(GetStreamId()), _rnd);
        }

        public int Size => _surface.Columns;
        public Surface.CellType[,] Cells => _surface.Cells;
    }

    public interface IEventStore
    {
        IEnumerable<Event> ReadStream(string getStreamId);
        void WriteEVents(string getStreamId, IEnumerable<Event> events);
    }

    public class BumpGame : Command
    {
        public BumpGame(string id, string causationId, string correlationId, DateTime createdOn) : base(id, causationId, correlationId, createdOn)
        {

        }
    }
}