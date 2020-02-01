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
        public IEnumerable<Event> Move(Surface.CellType[,] board, int numberOfRows, int numberOfColumns, Random rnd,
            Message cause)
        {
            var possibleMoves = this.GetSurroundingCells(1, numberOfRows, numberOfColumns);
            var validMoves = GetValidMoves(possibleMoves, board).ToList();
            if (!validMoves.Any())
            {
                return new Event[0];
            }
            var move = PickMove(validMoves, rnd);

            return new List<Event>() {new HerbivoreMoved(this, move, cause)};
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
}