namespace TestDrivingFun.Engine
{
    public class CreatePlantAccepted : Event
    {
        public int X { get; }
        public int Y { get; }

        public CreatePlantAccepted(in int x, in int y, Message cause) : base(GetId(typeof(CreatePlantAccepted)), cause)
        {
            X = x;
            Y = y;
        }
    }
}