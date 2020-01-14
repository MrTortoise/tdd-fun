namespace TestDrivingFun.Engine
{
    public interface IApply<in T> where T : Event
    {
        void Apply(T @event);
    }
}