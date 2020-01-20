using System;
using Newtonsoft.Json;

namespace TestDrivingFun.Engine
{
    public class CarnivoreAteHerbivore : Event
    {
        [JsonConstructor]
        public CarnivoreAteHerbivore(string carnivoreId, Coordinate oldPosition, Coordinate newPosition, string id,
            string causationId, string correlationId, DateTime createdOn) : base(id, causationId, correlationId,
            createdOn)
        {
            CarnivoreId = carnivoreId;
            OldPosition = oldPosition;
            NewPosition = newPosition;
        }

        public CarnivoreAteHerbivore(Carnivore carnivore, in Coordinate herbivoreCoordinate, Message cause) : base(GetId(typeof(CarnivoreAteHerbivore)),cause)
        {
            CarnivoreId = carnivore.Id;
            OldPosition = new Coordinate(carnivore.X, carnivore.Y);
            NewPosition = herbivoreCoordinate;
        }

        public Coordinate NewPosition { get; set; }
        public Coordinate OldPosition { get; set; }
        public string CarnivoreId { get; }
    }
}