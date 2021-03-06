using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace TestDrivingFun.Engine
{
    public class Surface : IAggregate,
        IHandle<CreateBoard>,
        IHandle<CreateHerbivore>,
        IHandle<CreateCarnivore>,
        IHandle<CreatePlants>,
        IHandle<BumpGame>
    {
        private readonly Random _random;

        private class SurfaceState :
            IApply<CreateBoardAccepted>,
            IApply<CreateHerbivoreAccepted>,
            IApply<CreateCarnivoreAccepted>,
            IApply<CreatePlantAccepted>,
            IApply<CarnivoreMoved>,
            IApply<HerbivoreMoved>,
            IApply<CarnivoreAteHerbivore>,
            IApply<CarnivoreDied>,
            IApply<HerbivoreLaidEgg>
        {
            public CellType[,] Cells { get; private set; } = new CellType[0, 0];

            public int Columns { get; private set; }

            public int Rows { get; private set; }

            public Dictionary<string, Carnivore> Carnivores { get; } = new Dictionary<string, Carnivore>();
            public Dictionary<string, Herbivore> Herbivores { get; } = new Dictionary<string, Herbivore>();

            public List<HerbivoreEgg> HerbivoreEggs { get; } = new List<HerbivoreEgg>();

            public void Apply(CreateBoardAccepted @event)
            {
                Rows = @event.X;
                Columns = @event.Y;
                Cells = new CellType[@event.X, @event.Y];
                for (var j = 0; j < @event.Y; j++)
                {
                    for (var i = 0; i < @event.X; i++)
                    {
                        Cells[i, j] = CellType.Default;
                    }
                }
            }

            public void Apply(CreateHerbivoreAccepted @event)
            {
                Cells[@event.X, @event.Y] = CellType.Herbivore;
                Herbivores.Add(@event.HerbivoreId, new Herbivore(@event.X, @event.Y, @event.HerbivoreId));
            }

            public void Apply(CreateCarnivoreAccepted @event)
            {
                Cells[@event.X, @event.Y] = CellType.Carnivore;
                Carnivores.Add(@event.CarnivoreId, new Carnivore(@event.X, @event.Y, @event.CarnivoreId, @event.MovesUntilDeath));
            }

            public void Apply(CreatePlantAccepted @event)
            {
                Cells[@event.X, @event.Y] = CellType.Plants;
            }

            public void Apply(CarnivoreMoved @event)
            {
                Cells[@event.OldPosition.X, @event.OldPosition.Y] = CellType.Default;
                Cells[@event.NewPosition.X, @event.NewPosition.Y] = CellType.Carnivore;
                
                Carnivores[@event.CarnivoreId].MoveTo(@event.NewPosition);
            }

            public void Apply(HerbivoreMoved @event)
            {
                Cells[@event.OldPosition.X, @event.OldPosition.Y] = CellType.Default;
                Cells[@event.NewPosition.X, @event.NewPosition.Y] = CellType.Herbivore;
                
                Herbivores[@event.HerbivoreId].SetPosition(@event.NewPosition);
            }

            public void Apply(CarnivoreAteHerbivore @event)
            {
                var carnivore = Carnivores[@event.CarnivoreId];
                var herbivore = Herbivores.Values.Single(h => h.X == @event.NewPosition.X && h.Y == @event.NewPosition.Y);

                Cells[carnivore.X, carnivore.Y] = CellType.Default;
                carnivore.Eat(herbivore);
                Herbivores.Remove(herbivore.Id);
                Cells[herbivore.X, herbivore.Y] = CellType.Carnivore;
            }

            public void Apply(CarnivoreDied @event)
            {
                var carnivore = Carnivores[@event.CarnivoreId];
                Carnivores.Remove(@event.CarnivoreId);

                Cells[carnivore.X, carnivore.Y] = CellType.Default;
            }

            public void Apply(HerbivoreLaidEgg @event)
            {
                HerbivoreEggs.Add(new HerbivoreEgg(@event.EggId, @event.X, @event.Y));
                Cells[@event.X, @event.Y] = CellType.HerbivoreEgg;
            }
        }

        private static readonly List<Type> ImplementedHandlers = typeof(Surface)
            .GetInterfaces()
            .Where(i => i.Name == typeof(IHandle<>).Name)
            .SelectMany(i => i.GenericTypeArguments)
            .ToList();

        private static readonly List<Type> ImplementedApplys = typeof(SurfaceState).GetInterfaces()
            .Where(i => i.Name == typeof(IApply<>).Name)
            .SelectMany(i => i.GetGenericArguments())
            .ToList();

        private readonly SurfaceState _state;

        public int Rows => _state.Rows;
        public int Columns => _state.Columns;
        public CellType[,] Cells => _state.Cells;

        public Surface(IEnumerable<Event> events, Random random)
        {
            _random = random;
            _state = new SurfaceState();
            foreach (var e in events)
            {
                var eventType = e.GetType();
                var exists = ImplementedApplys.Exists(a => a == eventType);
                if (!exists) throw new NotImplementedException($"Surface state does not implement handler for {eventType.Name}");

                _state.Apply((dynamic)e);
            }
        }

        public IEnumerable<Event> Handle(Command command)
        {
            foreach (var type in ImplementedHandlers)
            {
                if (command.GetType() == type)
                {
                    return Handle((dynamic)command);
                }
            }

            throw new NotImplementedException($"No handler in Surface for {command.GetType()}");
        }

        public IEnumerable<Event> Handle(CreateBoard command)
        {
            return new List<Event>()
            {
                new CreateBoardAccepted(command)
            };
        }

        public IEnumerable<Event> Handle(CreateHerbivore command)
        {
            CheckPositionIsDefault(command);
            return new List<CreateHerbivoreAccepted>
            {
                new CreateHerbivoreAccepted(command.HerbivoreId, command.X, command.Y, command)
            };
        }


        public IEnumerable<Event> Handle(CreateCarnivore command)
        {
            CheckPositionIsDefault(command);
            return new List<CreateCarnivoreAccepted>
            {
                new CreateCarnivoreAccepted(command.CarnivoreId, command.X, command.Y,command.MovesUntilDeath, command)
            };
        }

        public IEnumerable<Event> Handle(CreatePlants command)
        {
            CheckPositionIsDefault(command);
            return new List<CreatePlantAccepted>
            {
                new CreatePlantAccepted(command.PlantId,  command.X, command.Y, command)
            };
        }

        public IEnumerable<Event> Handle(BumpGame command)
        {
            foreach (var carnivore in _state.Carnivores.Values)
            {
                var events = carnivore.Move(_state.Cells, Rows, Columns, _random, command).ToList();
                if (!events.Any())
                {
                    continue;
                }

                foreach (var @event in events)
                {
                    _state.Apply((dynamic)@event);
                    yield return @event;
                }
            }

            foreach (var herbivore in _state.Herbivores.Values)
            {
                var events = herbivore.Move(_state.Cells, Rows, Columns, _random, command).ToList();
                if (!events.Any())
                {
                    continue;
                }
                foreach (var @event in events)
                {
                    _state.Apply((dynamic)@event);
                    yield return @event;
                }
            }
        }

        private void CheckPositionIsDefault(IHaveCoordinates toCheck)
        {
            if (_state.Cells[toCheck.X, toCheck.Y] != CellType.Default)
            {
                throw new PositionAlreadyTakenException(toCheck.X, toCheck.Y, _state.Cells[toCheck.X, toCheck.Y]);
            }
        }

        public enum CellType
        {
            Default,
            Herbivore,
            Carnivore,
            Plants,
            HerbivoreEgg
        }

        public class PositionAlreadyTakenException : Exception
        {
            public PositionAlreadyTakenException(in int x, in int y, CellType stateCell) : base($"attempted to create something at [{x},{y}] but its {stateCell}")
            {
            }
        }
    }

    public class HerbivoreEgg
    {
        public string Id { get; }
        public int X { get; }
        public int Y { get; }

        public HerbivoreEgg(string id, in int x, in int y)
        {
            Id = id;
            X = x;
            Y = y;
        }
    }
}