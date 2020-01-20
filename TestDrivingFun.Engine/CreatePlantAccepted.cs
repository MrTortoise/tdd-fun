using System;
using Newtonsoft.Json;

namespace TestDrivingFun.Engine
{
    public class CreatePlantAccepted : Event
    {
        public string PlantId { get; }
        public int X { get; }
        public int Y { get; }

        [JsonConstructor]
        public CreatePlantAccepted(string plantId, int x, int y, string id, string causationId,
            string correlationId, DateTime createdOn) : base(id, causationId, correlationId, createdOn)
        {
            PlantId = plantId;
            X = x;
            Y = y;
        }

        public CreatePlantAccepted(string plantId, in int x, in int y, Message cause) : base(GetId(typeof(CreatePlantAccepted)), cause)
        {
            PlantId = plantId;
            X = x;
            Y = y;
        }
    }
}