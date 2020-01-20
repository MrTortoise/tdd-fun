using System;

namespace TestDrivingFun.Engine
{
    public class CreatePlants : Command, IHaveCoordinates
    {
        public CreatePlants(int x, int y, string plantId, string id) : base(id, id, id, DateTime.Now)
        {
            X = x;
            Y = y;
            PlantId = plantId;
        }

        public int X { get; }
        public int Y { get; }
        public string PlantId { get; }
    }
}