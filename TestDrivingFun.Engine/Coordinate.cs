using Newtonsoft.Json;

namespace TestDrivingFun.Engine
{
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
    }
}