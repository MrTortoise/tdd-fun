using System;
using Newtonsoft.Json;

namespace TestDrivingFun.Engine
{
    public class CarnivoreDied : Event
    {
        public string CarnivoreId { get; }

        [JsonConstructor]
        public CarnivoreDied(string carnivoreId, string id, string causationId, string correlationId, DateTime createdOn) : base(id, causationId, correlationId, createdOn)
        {
            CarnivoreId = carnivoreId;
        }

        public CarnivoreDied(string carnivoreId, Message cause) : base(GetId(typeof(CarnivoreDied)), cause)
        {
            CarnivoreId = carnivoreId;
        }
    }
}