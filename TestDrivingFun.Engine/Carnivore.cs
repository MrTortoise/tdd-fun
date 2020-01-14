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

        public int X { get; }
        public int Y { get; }
        public string Id { get; }
    }
}