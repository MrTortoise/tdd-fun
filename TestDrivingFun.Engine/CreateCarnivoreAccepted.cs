using Newtonsoft.Json;

namespace TestDrivingFun.Engine
{
    public class CreateCarnivoreAccepted : Event
    {
        public int X { get; }
        public int Y { get; }
        public string CarnivoreId { get; }
        public int MovesUntilDeath { get; }

        [JsonConstructor]
        public CreateCarnivoreAccepted(string carnivoreId, int x, int y, int movesUntilDeath, Message cause) : base(GetId(typeof(CreateCarnivoreAccepted)), cause)
        {
            X = x;
            Y = y;
            MovesUntilDeath = movesUntilDeath;
            CarnivoreId = carnivoreId;
        }
    }
}