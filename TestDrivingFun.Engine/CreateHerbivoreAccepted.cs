using System;

namespace TestDrivingFun.Engine
{
    public class CreateHerbivoreAccepted : Event
    {
        public int X { get; }
        public int Y { get; }

        public CreateHerbivoreAccepted(int x, int y, string id, string causationId, string correlationId, DateTime createdOn) : base(id, causationId, correlationId, createdOn)
        {
            X = x;
            Y = y;
        }

        public CreateHerbivoreAccepted(int x, int y, Message cause) : base(GetId(typeof(CreateHerbivoreAccepted)), cause)
        {
            X = x;
            Y = y;
        }
    }
}