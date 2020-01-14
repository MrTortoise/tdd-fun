using System;
using System.Collections.Generic;
using System.Linq;

namespace TestDrivingFun.Engine
{
    public class Surface : IAggregate, IHandle<CreateBoard>, IHandle<CreateHerbivore>, IHandle<CreateCarnivore>, IHandle<CreatePlants>
    {
        private class SurfaceState : IApply<CreateBoardAccepted>, IApply<CreateHerbivoreAccepted>, IApply<CreateCarnivoreAccepted>, IApply<CreatePlantAccepted>
        {
            public CellType[,] Cells { get; private set; }

            public int Columns { get; private set; }

            public int Rows { get; private set; }

            public void Apply(CreateBoardAccepted @event)
            {
                Rows = @event.Y;
                Columns = @event.X;
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
            }

            public void Apply(CreateCarnivoreAccepted @event)
            {
                Cells[@event.X, @event.Y] = CellType.Carnivore;
            }

            public void Apply(CreatePlantAccepted @event)
            {
                Cells[@event.X, @event.Y] = CellType.Plants;
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

        public Surface(IEnumerable<Event> events)
        {
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
                new CreateHerbivoreAccepted(command.X, command.Y, command)
            };
        }


        public IEnumerable<Event> Handle(CreateCarnivore command)
        {
            CheckPositionIsDefault(command);
            return new List<CreateCarnivoreAccepted>
            {
                new CreateCarnivoreAccepted(command.X, command.Y, command)
            };
        }

        public IEnumerable<Event> Handle(CreatePlants command)
        {
            CheckPositionIsDefault(command);
            return new List<CreatePlantAccepted>
            {
                new CreatePlantAccepted(command.X, command.Y, command)
            };
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
            Plants
        }

        public class PositionAlreadyTakenException : Exception
        {
            public PositionAlreadyTakenException(in int x, in int y, CellType stateCell) : base($"attempted to create something at [{x},{y}] but its {stateCell}")
            {
            }
        }
    }
}