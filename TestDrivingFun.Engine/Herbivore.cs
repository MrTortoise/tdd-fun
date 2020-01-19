using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace TestDrivingFun.Engine
{
    public class Herbivore : IHaveCoordinates, IMove
    {
        public Herbivore(int x, int y, string id)
        {
            X = x;
            Y = y;
            Id = id;
        }

        public int X { get; private set; }
        public int Y { get; private set; }
        public string Id { get; }
        public Event Move(Surface.CellType[,] board, int numberOfRows, int numberOfColumns, Random rnd, Message cause)
        {
            var possibleMoves = this.GetSurroundingCells(1, numberOfRows, numberOfColumns);
            var validMoves = GetValidMoves(possibleMoves, board).ToList();
            if (!validMoves.Any())
            {
                return Event.None;
            }
            var move = PickMove(validMoves, rnd);

            return new HerbivoreMoved(this, move, cause);
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

        public void SetPosition(Coordinate newPosition)
        {
            X = newPosition.X;
            Y = newPosition.Y;
        }
    }

    public class HerbivoreMoved : Event
    {
        public HerbivoreMoved(Herbivore herbivore, Coordinate move, Message cause) : base(GetId(typeof(HerbivoreMoved)),
            cause)
        {
            OldPosition = new Coordinate(herbivore.X, herbivore.Y);
            NewPosition = move;
            HerbivoreId = herbivore.Id;
        }

        public string HerbivoreId { get; set; }

        public Coordinate NewPosition { get; set; }

        public Coordinate OldPosition { get; set; }

        [JsonConstructor]
        public HerbivoreMoved(string id, string herbivoreId, Coordinate oldPosition, Coordinate newPosition, string causationId,
            string correlationId, DateTime createdOn) : base(id, causationId, correlationId, createdOn)
        {
            HerbivoreId = herbivoreId;
            OldPosition = oldPosition;
            NewPosition = newPosition;
        }
    }
}