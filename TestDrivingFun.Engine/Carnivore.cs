using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace TestDrivingFun.Engine
{
    public class Carnivore : IHaveCoordinates
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
            var possibleMoves = GetSurroundingCells(X, Y, 1, numberOfRows, numberOfColumns);
            var validMoves = GetValidMoves(possibleMoves, board).ToList();
            if (!validMoves.Any())
            {
                return new CarnivoreDidNotMove(this, cause);
            }
            var move = PickMove(validMoves, rnd);

            return new CarnivoreMoved(this, move, cause);
        }

        private Coordinate PickMove(IEnumerable<Coordinate> validMoves, Random rnd)
        {
            var coordinates = validMoves.ToList();
            var randomMove = rnd.Next(0, coordinates.Count);
            return coordinates[randomMove];
        }

        private IEnumerable<Coordinate> GetValidMoves(IEnumerable<Coordinate> possibleMoves, Surface.CellType[,] board)
        {
            return possibleMoves.Where(
                possibleMove => board[possibleMove.X, possibleMove.Y] == Surface.CellType.Default);
        }

        private IEnumerable<Coordinate> GetSurroundingCells(
            int x,
            int y,
            int distance,
            int numberOfRows,
            int numberOfColumns)
        {
            var minX = x - distance;
            var maxX = x + distance;
            var minY = y - distance;
            var maxY = y + distance;

            if (minX < 0)
            {
                minX = 0;
            }

            if (minY < 0)
            {
                minY = 0;
            }

            if (maxX > numberOfColumns - 1)
            {
                maxX = numberOfColumns - 1;
            }

            if (maxY > numberOfRows - 1)
            {
                maxY = numberOfRows - 1;
            }


            for (var i = minX; i <= maxX; i++)
            {
                for (var j = minY; j <= maxY; j++)
                {
                    if (i == x && j == y) continue;

                    yield return new Coordinate(i, j);
                }
            }
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