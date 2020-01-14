using System;

namespace TestDrivingFun.Engine
{
    public class CreateHerbivore : Command, IHaveCoordinates
    {
        public CreateHerbivore(int x, int y, string id) : base(id, id, id, DateTime.Now)
        {
            X = x;
            Y = y;
        }

        public CreateHerbivore(Herbivore herbivore, string causationId, string correlationId) : base(herbivore.Id, causationId, correlationId, DateTime.Now)
        {
            X = herbivore.X;
            Y = herbivore.Y;
        }

        public int X { get; }
        public int Y { get; }
    }

    public interface IHaveCoordinates
    {
        int X { get; }
        int Y { get; }
    }
}