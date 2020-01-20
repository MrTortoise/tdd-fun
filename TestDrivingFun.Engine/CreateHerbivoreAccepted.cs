using System;
using Newtonsoft.Json;

namespace TestDrivingFun.Engine
{
    public class CreateHerbivoreAccepted : Event
    {
        public int X { get; }
        public int Y { get; }
        public string HerbivoreId { get; }

        [JsonConstructor]
        public CreateHerbivoreAccepted(string herbivoreId, int x, int y, string id, string causationId,
            string correlationId, DateTime createdOn) : base(id, causationId, correlationId, createdOn)
        {
            X = x;
            Y = y;
            HerbivoreId = herbivoreId;
        }

        public CreateHerbivoreAccepted(string herbivoreId, int x, int y, Message cause) : base(GetId(typeof(CreateHerbivoreAccepted)), cause)
        {
            if (string.IsNullOrWhiteSpace(herbivoreId))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(herbivoreId));
            X = x;
            Y = y;
            HerbivoreId = herbivoreId;
        }
    }
}