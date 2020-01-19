using System;
using System.Collections.Generic;
using System.Text;
using TestDrivingFun.Engine;
using Xunit;

namespace testDrivingFun.Spec
{
    public class SurvivalGameWill
    {
        [Fact]
        public void CreateAGameWithABoardAndStuff()
        {
            var ut = new SurvivalGame(new Random());
            var createHerbivores = new List<Herbivore>();
            for (int i = 1; i <= 10; i++)
            {
                createHerbivores.Add(new Herbivore(0, i, "h" + i));
            }

            var createCarnivores = new List<Carnivore>();
            for (int i = 1; i <= 10; i++)
            {
                createCarnivores.Add(new Carnivore(19, i, "c" + i));
            }

            ut.CreateNewGame(20, createHerbivores, createCarnivores);
        }
    }
}
