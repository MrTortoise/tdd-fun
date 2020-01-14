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
            var ut = new SurvivalGame();
            var createHerbivores = new List<Herbivore>()
            {
                new Herbivore(0,1,  String.Empty),
                new Herbivore(0,2, String.Empty),
                new Herbivore(0,3, String.Empty),
                new Herbivore(0,4, String.Empty),
                new Herbivore(0,5, String.Empty),
                new Herbivore(0,6, String.Empty),
                new Herbivore(0,7, String.Empty),
                new Herbivore(0,8, String.Empty),
                new Herbivore(0,9, String.Empty),
                new Herbivore(0,10, String.Empty)
            };
            var createCarnivores = new List<Carnivore>()
            {
                new Carnivore(19, 1, String.Empty),
                new Carnivore(19, 2, String.Empty),
                new Carnivore(19, 3, String.Empty),
                new Carnivore(19, 4, String.Empty),
                new Carnivore(19, 5, String.Empty),
                new Carnivore(19, 6, String.Empty),
                new Carnivore(19, 7, String.Empty),
                new Carnivore(19, 8, String.Empty),
                new Carnivore(19, 9, String.Empty),
                new Carnivore(19, 10, String.Empty)
            };
            ut.CreateNewGame(20, createHerbivores, createCarnivores);
        }
    }
}
