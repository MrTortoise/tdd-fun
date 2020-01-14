using System.Collections.Generic;

namespace TestDrivingFun.Engine
{
    public interface IHandle<T> where T : Command
    {
        IEnumerable<Event> Handle(T command);
    }
}