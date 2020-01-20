using System;
using Newtonsoft.Json;

namespace TestDrivingFun.Engine
{
    public class HerbivoreMoved : Event
    {
        public HerbivoreMoved(Herbivore herbivore, Coordinate move, Message cause) : base(GetId(typeof(HerbivoreMoved)),
            cause)
        {
            OldPosition = new Coordinate(herbivore.X, herbivore.Y);
            NewPosition = move;
            HerbivoreId = herbivore.Id;
        }

        public string HerbivoreId { get; set; }

        public Coordinate NewPosition { get; set; }

        public Coordinate OldPosition { get; set; }

        [JsonConstructor]
        public HerbivoreMoved(string id, string herbivoreId, Coordinate oldPosition, Coordinate newPosition, string causationId,
            string correlationId, DateTime createdOn) : base(id, causationId, correlationId, createdOn)
        {
            HerbivoreId = herbivoreId;
            OldPosition = oldPosition;
            NewPosition = newPosition;
        }
    }
}