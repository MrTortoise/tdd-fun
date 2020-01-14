using System.Collections.Generic;

namespace TestDrivingFun.Engine
{
    public interface IAggregate
    {
        IEnumerable<Event> Handle(Command command);
    }
}