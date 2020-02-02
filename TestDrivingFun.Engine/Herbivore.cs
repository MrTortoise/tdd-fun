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
          
            var moves = PickMove(possibleMoves, rnd, board, numberOfRows, numberOfColumns, cause);

            return moves;
        }

        private IEnumerable<Event> PickMove(IEnumerable<Coordinate> validMoves, Random rnd, Surface.CellType[,] board,
            int numberOfRows, int numberOfColumns, Message cause)
        {
            var coordinates = validMoves.ToList();

            var herbivoresToMateWith =
                coordinates
                    .Where(m => board[m.X, m.Y] == Surface.CellType.Herbivore)
                    .ToList();

            if (herbivoresToMateWith.Any())
            {
                var herbivore = herbivoresToMateWith.First();
                var vectorX = X - herbivore.X;
                var vectorY = Y - herbivore.Y;
                var layEggEvents = new List<Event>();
                var newCoordinate = new Coordinate(X + vectorX, Y + vectorY);
                if (IsValid(newCoordinate, board, numberOfRows, numberOfColumns))
                {
                    layEggEvents.Add(new HerbivoreMoved(this, newCoordinate, cause));
                }
                else
                {
                    layEggEvents.Add(GetAnyValidMove(rnd, board, cause, coordinates));
                }

                layEggEvents.Add(new HerbivoreLaidEgg(this, cause));
                return layEggEvents;
            }

            return new List<Event>() {GetAnyValidMove(rnd, board, cause, coordinates)};
        }

        private Event GetAnyValidMove(Random rnd, Surface.CellType[,] board, Message cause, List<Coordinate> coordinates)
        {
            var emptyMoves = coordinates.Where(c => board[c.X, c.Y] == Surface.CellType.Default).ToList();
            var randomMove = rnd.Next(0, emptyMoves.Count);
            return new HerbivoreMoved(this, emptyMoves[randomMove], cause);
        }

        private bool IsValid(Coordinate coordinate, Surface.CellType[,] board, int numberOfRows, int numberOfColumns)
        {
            if (coordinate.X < 0 || coordinate.Y < 0) return false;
            if (coordinate.X > numberOfColumns - 1 || coordinate.Y > numberOfRows - 1) return false;

            return board[coordinate.X, coordinate.Y] == Surface.CellType.Default;
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