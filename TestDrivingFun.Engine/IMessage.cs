using System;

namespace TestDrivingFun.Engine
{
    public class Message
    {
        public Message(string newId, Message cause) 
            :this(newId, cause.Id, cause.CorrelationId, DateTime.Now)
        {
        }
        public Message(string id, string causationId, string correlationId, DateTime createdOn)
        {
            Id = id;
            CausationId = causationId;
            CorrelationId = correlationId;
            CreatedOn = createdOn;
        }

        public string Id { get; }
        public string CausationId { get; }
        public string CorrelationId { get; }
        public DateTime CreatedOn { get; }
    }
}