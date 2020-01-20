using System.Collections.Generic;

namespace TestDrivingFun.Engine
{
    public interface IHandle<in T> where T : Command
    {
        IEnumerable<Event> Handle(T command);
    }
}