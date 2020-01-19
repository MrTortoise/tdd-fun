using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace TestDrivingFun.Engine
{
    public interface IMove
    {
        Event Move(Surface.CellType[,] board, int numberOfRows, int numberOfColumns, Random rnd, Message cause);
    }

    public class Carnivore : IHaveCoordinates, IMove
    {
        public Carnivore(int x, int y, string id)
        {
            X = x;
            Y = y;
            Id = id;
        }

        public int X { get; set; }
        public int Y { get; set; }
        public string Id { get; }
        public IMovementType MovementType { get; } = new MoveOneSpaceRule();

        public Event Move(Surface.CellType[,] board, int numberOfRows, int numberOfColumns, Random rnd, Message cause)
        {
            var possibleMoves = this.GetSurroundingCells(1, numberOfRows, numberOfColumns);
            var validMoves = GetValidMoves(possibleMoves, board).ToList();
            if (!validMoves.Any())
            {
                return Event.None;
            }
            return PickMove(validMoves, rnd, board, cause);
        }

        private Event PickMove(IEnumerable<Coordinate> validMoves, Random rnd, Surface.CellType[,] board, Message cause)
        {
            var coordinates = validMoves.ToList();

            var herbivoresToEat = coordinates.Where(m =>
            {
                var cellType = board[m.X, m.Y];
                return cellType == Surface.CellType.Herbivore;
            }).ToList();
            if (herbivoresToEat.Any())
            {
                var randomHerbivore = rnd.Next(0, herbivoresToEat.Count());
                return new CarnivoreAteHerbivore(this, herbivoresToEat[randomHerbivore], cause);
            }

            var randomMove = rnd.Next(0, coordinates.Count);
            return new CarnivoreMoved(this, coordinates[randomMove], cause);
        }

        private IEnumerable<Coordinate> GetValidMoves(IEnumerable<Coordinate> possibleMoves, Surface.CellType[,] board)
        {
            return possibleMoves.Where(
                possibleMove => board[possibleMove.X, possibleMove.Y] != Surface.CellType.Carnivore);
        }

      


        public class MoveOneSpaceRule : IMovementType
        {
        }

        public interface IMovementType
        {
        }

        public void SetPosition(IHaveCoordinates newPosition)
        {
            X = newPosition.X;
            Y = newPosition.Y;
        }
    }

    public class CarnivoreDidNotMove : Event
    {
        public CarnivoreDidNotMove(Carnivore carnivore, Message cause) : base(GetId(typeof(CarnivoreDidNotMove)), cause)
        {
            CarnivoreId = carnivore.Id;
        }

        public string CarnivoreId { get; set; }
    }

    public class CarnivoreMoved : Event
    {
        public CarnivoreMoved(Carnivore carnivore, Coordinate move, Message cause) : base(GetId(typeof(CarnivoreMoved)),
            cause)
        {
            OldPosition = new Coordinate(carnivore.X, carnivore.Y);
            NewPosition = move;
            CarnivoreId = carnivore.Id;
        }

        [JsonConstructor]
        public CarnivoreMoved(string id, string carnivoreId, Coordinate oldPosition, Coordinate newPosition, string causationId,
            string correlationId, DateTime createdOn) : base(id, causationId, correlationId, createdOn)
        {
            CarnivoreId = carnivoreId;
            OldPosition = oldPosition;
            NewPosition = newPosition;
        }

        public string CarnivoreId { get; set; }
        public Coordinate OldPosition { get; set; }
        public Coordinate NewPosition { get; set; }
    }
}