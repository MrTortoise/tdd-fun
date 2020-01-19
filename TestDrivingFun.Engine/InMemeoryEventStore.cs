using System.Collections.Generic;


namespace TestDrivingFun.Engine
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly Dictionary<string,List<Event>> _events = new Dictionary<string, List<Event>>();

        public IEnumerable<Event> ReadStream(string streamName)
        {
          CreateIfNotExist(streamName);
          return _events[streamName];
        }

        public void WriteEVents(string streamName, IEnumerable<Event> events)
        {
            CreateIfNotExist(streamName);

            _events[streamName].AddRange(events);
        }

        private void CreateIfNotExist(string streamName)
        {
            if (!_events.ContainsKey(streamName))
            {
                _events.Add(streamName, new List<Event>());
            }
        }
    }
}