using System;

namespace TestDrivingFun.Engine
{
    public class Command : Message
    {
        public Command(string id, string causationId, string correlationId, DateTime createdOn) : base(id, causationId, correlationId, createdOn)
        {
        }

    }
}