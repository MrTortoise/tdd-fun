using System;
using System.Collections.Generic;

namespace TestDrivingFun.Engine
{
    public class CreateBoardAccepted : Event
    {
        public int X { get; }
        public int Y { get; }
        public IEnumerable<Herbivore> Herbivores { get; }
        public IEnumerable<Carnivore> Carnivores { get; }

        public CreateBoardAccepted(int x, int y, IEnumerable<Herbivore> herbivores, IEnumerable<Carnivore> carnivores, string causationId,
            string correlationId, DateTime createdOn) : base(Event.GetId(typeof(CreateBoardAccepted)), causationId, correlationId, createdOn  )
        {
            X = x;
            Y = y;
            Herbivores = herbivores;
            Carnivores = carnivores;
        }

        public CreateBoardAccepted(CreateBoard command) : base(GetId(typeof(CreateBoardAccepted)), command.CausationId, command.CorrelationId, DateTime.Now)
        {
            X = command.X;
            Y = command.Y;
            Herbivores = command.Herbivores;
            Carnivores = command.Carnivores;

        }
    }
}