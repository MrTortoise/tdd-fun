using System;
using Newtonsoft.Json;

namespace TestDrivingFun.Engine
{
    public class CarnivoreMoved : Event
    {
        public CarnivoreMoved(Carnivore carnivore, Coordinate move, Message cause) : base(GetId(typeof(CarnivoreMoved)),
            cause)
        {
            OldPosition = new Coordinate(carnivore.X, carnivore.Y);
            NewPosition = move;
            CarnivoreId = carnivore.Id;
        }

        [JsonConstructor]
        public CarnivoreMoved(string id, string carnivoreId, Coordinate oldPosition, Coordinate newPosition, string causationId,
            string correlationId, DateTime createdOn) : base(id, causationId, correlationId, createdOn)
        {
            CarnivoreId = carnivoreId;
            OldPosition = oldPosition;
            NewPosition = newPosition;
        }

        public string CarnivoreId { get; set; }
        public Coordinate OldPosition { get; set; }
        public Coordinate NewPosition { get; set; }
    }
}