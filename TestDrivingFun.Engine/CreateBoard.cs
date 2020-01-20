
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace TestDrivingFun.Engine
{
    public class CreateBoard : Command
    {
        public int X { get; }
        public int Y { get; }
        public IEnumerable<Herbivore> Herbivores { get; }
        public IEnumerable<Carnivore> Carnivores { get; }

        [JsonConstructor]
        public CreateBoard(int x, int y, IEnumerable<Herbivore> herbivores, IEnumerable<Carnivore> carnivores,
            string id) : base(id, id, id, DateTime.Now)
        {
            X = x;
            Y = y;
            Herbivores = herbivores;
            Carnivores = carnivores;
        }
    }
}