namespace TestDrivingFun.Engine
{
    public class HerbivoreCreated : Event
    {
        public int X { get; }
        public int Y { get; }

        public HerbivoreCreated(int x, int y, Message cause) : base(GetId(typeof(HerbivoreCreated)), cause)
        {
            X = x;
            Y = y;
        }
    }
}