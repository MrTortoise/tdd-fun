using System;
using Newtonsoft.Json;

namespace TestDrivingFun.Engine
{
    public class CreateCarnivore : Command, IHaveCoordinates
    {
        public int X { get; }
        public int Y { get; }
        public string CarnivoreId { get; }
        public int MovesUntilDeath { get; }

        [JsonConstructor]
        public CreateCarnivore(string carnivoreId, int x, int y, int movesUntilDeath, string id) : base(id, id, id, DateTime.Now)
        {
            CarnivoreId = carnivoreId;
            X = x;
            Y = y;
            MovesUntilDeath = movesUntilDeath;
        }

        public CreateCarnivore(Carnivore carnivore, in string causationId, in string correlationId) : base(carnivore.Id, causationId, correlationId, DateTime.Now)
        {
            MovesUntilDeath = carnivore.MovesUntilDeath;
            X = carnivore.X;
            Y = carnivore.Y;
            CarnivoreId = carnivore.Id;
        }
    }
}