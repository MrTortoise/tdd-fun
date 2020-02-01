using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text.Json.Serialization;

namespace TestDrivingFun.Engine
{
    public interface IMove
    {
        IEnumerable<Event> Move(Surface.CellType[,] board, int numberOfRows, int numberOfColumns, Random rnd, Message cause);
    }

    public class Carnivore : IHaveCoordinates, IMove
    {
        public Carnivore(int x, int y, string id, int movesUntilDeath)
        {
            X = x;
            Y = y;
            Id = id;
            MovesLeftAfterEating = movesUntilDeath;
            MovesUntilDeath = movesUntilDeath;
        }

        public int MovesLeftAfterEating { get;  }

        public int X { get; private set; }
        public int Y { get; private set; }
        public string Id { get; }
        public int MovesUntilDeath { get; private set; }

        public IEnumerable<Event> Move(Surface.CellType[,] board, int numberOfRows, int numberOfColumns, Random rnd,
            Message cause)
        {
            var possibleMoves = this.GetSurroundingCells(1, numberOfRows, numberOfColumns);
            return PickMove(possibleMoves, rnd, board, cause, numberOfRows, numberOfColumns);
        }

        private IEnumerable<Event> PickMove(IEnumerable<Coordinate> validMoves, Random rnd, Surface.CellType[,] board, Message cause,
            int numberOfRows, int numberOfColumns)
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
                return new List<Event>() {new CarnivoreAteHerbivore(this, herbivoresToEat[randomHerbivore], cause)};
            }

            if (MovesUntilDeath == 1)
            {
                return new List<Event>() { new CarnivoreDied(Id, cause)};
            }

            var carnivoresToRunFrom = 
                coordinates
                    .Where(m => board[m.X, m.Y] == Surface.CellType.Carnivore)
                    .ToList();

            if (carnivoresToRunFrom.Any())
            {
                var carnivore = carnivoresToRunFrom.First();
                var vectorX = X - carnivore.X;
                var vectorY = Y - carnivore.Y;

                var newCoordinate = new Coordinate(X + vectorX, Y + vectorY);
                if (IsValid(newCoordinate, board, numberOfRows, numberOfColumns))
                {
                    return new List<Event>() { new CarnivoreMoved(this, newCoordinate, cause)};
                }
            }

            var emptyMoves = coordinates.Where(c => board[c.X, c.Y] == Surface.CellType.Default).ToList();
            var randomMove = rnd.Next(0, emptyMoves.Count);
            return new List<Event>() { new CarnivoreMoved(this, emptyMoves[randomMove], cause)};
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
                possibleMove => board[possibleMove.X, possibleMove.Y] != Surface.CellType.Carnivore);
        }


        public void MoveTo(IHaveCoordinates newPosition)
        {
            X = newPosition.X;
            Y = newPosition.Y;
            MovesUntilDeath--;
        }

        public void Eat(IHaveCoordinates herbivore)
        {
            X = herbivore.X;
            Y = herbivore.Y;
            MovesUntilDeath = MovesLeftAfterEating;
        }
    }
}