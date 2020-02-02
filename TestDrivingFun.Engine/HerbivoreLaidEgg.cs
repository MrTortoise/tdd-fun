using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace TestDrivingFun.Engine
{
    public class HerbivoreLaidEgg : Event
    {
        public HerbivoreLaidEgg(string eggId, int x, int y, string id, string causationId, string correlationId, DateTime createdOn) : base(id, causationId, correlationId, createdOn)
        {
            EggId = eggId;
            X = x;
            Y = y;
        }

        public HerbivoreLaidEgg(Herbivore herbivore, Message cause) : base(Event.GetId(typeof(HerbivoreLaidEgg)), cause)
        {
            X = herbivore.X;
            Y = herbivore.Y;
            EggId = GetId(typeof(HerbivoreLaidEgg));
        }

        public string EggId { get; set; }

        public int Y { get; set; }

        public int X { get; set; }
    }
}