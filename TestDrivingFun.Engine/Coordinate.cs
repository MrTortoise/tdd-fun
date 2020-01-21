using System.Collections.Generic;
using System.Diagnostics;
using Newtonsoft.Json;

namespace TestDrivingFun.Engine
{
    [DebuggerDisplay("{X},{Y}")]
    public class Coordinate : IHaveCoordinates
    {
        [JsonConstructor]
        public Coordinate(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }

        public static IEnumerable<Coordinate> GetSurroundingCells(
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

    }
}