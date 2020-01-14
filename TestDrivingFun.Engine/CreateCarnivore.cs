using System;

namespace TestDrivingFun.Engine
{
    public class CreateCarnivore : Command, IHaveCoordinates
    {
        public int X { get; }
        public int Y { get; }

        public CreateCarnivore(int x, int y, string id) : base(id, id, id, DateTime.Now)
        {
            X = x;
            Y = y;
        }

        public CreateCarnivore(Carnivore carnivore, in string causationId, in string correlationId) : base(carnivore.Id, causationId, correlationId, DateTime.Now)
        {
            throw new NotImplementedException();
        }
    }
}