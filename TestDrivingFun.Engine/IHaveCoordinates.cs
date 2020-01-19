using System.Collections.Generic;

namespace TestDrivingFun.Engine
{
    public interface IHaveCoordinates
    {
        int X { get; }
        int Y { get; }
    }

    public static class CoordinateExtension
    {
        public static IEnumerable<Coordinate> GetSurroundingCells(this IHaveCoordinates instance, int distance,
            int numberOfRows,
            int numberOfColumns)
        {
            return Coordinate.GetSurroundingCells(instance.X, instance.Y, distance, numberOfRows, numberOfColumns);
        }
    }
}