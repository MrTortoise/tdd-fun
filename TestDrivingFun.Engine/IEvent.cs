using System;

namespace TestDrivingFun.Engine
{
    public class Event : Message
    {
        public static string GetId(Type type)
        {
            return $"{type.FullName}-{Guid.NewGuid()}";
        }
        public Event(string id, string causationId, string correlationId, DateTime createdOn) : base(id, causationId, correlationId, createdOn)
        {
        }

        protected Event(string id, Message cause) : base(id, cause.Id, cause.CorrelationId, DateTime.Now)
        {
        }
    }
}