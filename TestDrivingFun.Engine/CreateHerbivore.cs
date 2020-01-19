using System;

namespace TestDrivingFun.Engine
{
    public class CreateHerbivore : Command, IHaveCoordinates
    {
        public CreateHerbivore(string herbivoreId, int x, int y, string id) : base(id, id, id, DateTime.Now)
        {
            X = x;
            Y = y;
            HerbivoreId = herbivoreId;
        }

        public CreateHerbivore(Herbivore herbivore, string causationId, string correlationId) : base(herbivore.Id, causationId, correlationId, DateTime.Now)
        {
            X = herbivore.X;
            Y = herbivore.Y;
            HerbivoreId = herbivore.Id;
        }

        public int X { get; }
        public int Y { get; }
        public string HerbivoreId { get; }
    }
}