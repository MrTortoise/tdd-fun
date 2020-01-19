namespace TestDrivingFun.Engine
{
    public class CreateCarnivoreAccepted : Event
    {
        public int X { get; }
        public int Y { get; }
        public string CarnivoreId { get; }

        public CreateCarnivoreAccepted(string carnivoreId, int x, int y, Message cause) : base(GetId(typeof(CreateCarnivoreAccepted)), cause)
        {
            X = x;
            Y = y;
            CarnivoreId = carnivoreId;
        }
    }
}