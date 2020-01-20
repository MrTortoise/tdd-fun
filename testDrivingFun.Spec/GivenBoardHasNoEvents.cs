using System;
using System.Collections.Generic;
using System.Linq;
using TestDrivingFun.Engine;
using Xunit;

namespace testDrivingFun.Spec
{
    public class GivenBoardHasNoEvents : EsTestBase
    {
        [Fact]
        public void CreateWithSize2020_WillHaveRightSizeAndNumberOfBeasties()
        {
            var given = new List<Event>();
            var herbivores = new List<Herbivore>
            {
                new Herbivore(1, 1,String.Empty)
            };
            var carnivores = new List<Carnivore>
            {
                new Carnivore(2, 2, String.Empty)
            };
            var when = new CreateBoard(20, 20, herbivores, carnivores, "woop");
            var then = new List<Event>
            {
                new CreateBoardAccepted(20, 20, new List<Herbivore>(), new List<Carnivore>(), Message.CausationId,
                    Message.CorrelationId, Message.CreatedOn)
            };

            var results = Execute(given, when, then, events => new Surface(events, new Random()));
            var e = (CreateBoardAccepted) results[0];
            Assert.Equal(20, e.X);
            Assert.Equal(20, e.Y);
        }
    }

    public class GivenBoardCreated : EsTestBase
    {
        [Fact]
        public void CreateCarnivores_HaveRightLocation()
        {
            var given = new List<Event>
            {
                new CreateBoardAccepted(20, 20, new List<Herbivore>(), new List<Carnivore>(), Message.CausationId,
                    Message.CorrelationId, Message.CreatedOn)
            };

            var when = new CreateCarnivore("testivore", 2, 3, "woop2");
            var then = new List<Event>
            {
                new CreateCarnivoreAccepted("testivore",2, 3, Message)
            };

            var results = Execute(given, when, then, events => new Surface(events, new Random()));

            var carnivores = results.Select(r => (CreateCarnivoreAccepted) r).ToList();
            foreach (var carnivore in carnivores)
                Assert.Single(carnivores, c => c.X == carnivore.X && c.Y == carnivore.Y);
        }

        [Fact]
        public void CreateCarnivores_WillFailIfPositionAlreadyTaken()
        {
            var given = new List<Event>
            {
                new CreateBoardAccepted(20, 20, new List<Herbivore>(), new List<Carnivore>(), Message.CausationId,
                    Message.CorrelationId, Message.CreatedOn),
                new CreateCarnivoreAccepted("testivore",2, 3, Message)
            };

            var when = new CreateCarnivore("testivore",2, 3, "woop2");

            Execute(given, when, typeof(Surface.PositionAlreadyTakenException), events => new Surface(events, new Random()));
        }

        [Fact]
        public void CreateHerbivores_HaveRightLocation()
        {
            var given = new List<Event>
            {
                new CreateBoardAccepted(20, 20, new List<Herbivore>(), new List<Carnivore>(), Message.CausationId,
                    Message.CorrelationId, Message.CreatedOn)
            };

            var when = new CreateHerbivore("testivore", 2, 3, "woop2");
            var then = new List<Event>
            {
                new CreateHerbivoreAccepted("testivore",2, 3, Message)
            };

            var results = Execute(given, when, then, events => new Surface(events, new Random()));

            var carnivores = results.Select(r => (CreateHerbivoreAccepted) r).ToList();
            foreach (var carnivore in carnivores)
                Assert.Single(carnivores, c => c.X == carnivore.X && c.Y == carnivore.Y);
        }

        [Fact]
        public void CreateHerbivores_WillFailIfPositionAlreadyTaken()
        {
            var given = new List<Event>
            {
                new CreateBoardAccepted(20, 20, new List<Herbivore>(), new List<Carnivore>(), Message.CausationId,
                    Message.CorrelationId, Message.CreatedOn),
                new CreateHerbivoreAccepted("testivore", 2, 3, Message)
            };

            var when = new CreateHerbivore("testivore", 2, 3, "woop2");

            Execute(given, when, typeof(Surface.PositionAlreadyTakenException), events => new Surface(events, new Random()));
        }

        [Fact]
        public void CreatePlants_HaveRightLocation()
        {
            var given = new List<Event>
            {
                new CreateBoardAccepted(20, 20, new List<Herbivore>(), new List<Carnivore>(), Message.CausationId,
                    Message.CorrelationId, Message.CreatedOn)
            };

            var when = new CreatePlants(2, 3, "test_plant",Message.Id);
            var then = new List<Event>
            {
                new CreatePlantAccepted("test_plant",2, 3, Message)
            };

            var results = Execute(given, when, then, events => new Surface(events, new Random()));

            var carnivores = results.Select(r => (CreatePlantAccepted) r).ToList();
            foreach (var carnivore in carnivores)
                Assert.Single(carnivores, c => c.X == carnivore.X && c.Y == carnivore.Y);
        }

        [Fact]
        public void CreatePlants_WillFailIfPositionAlreadyTaken()
        {
            var given = new List<Event>
            {
                new CreateBoardAccepted(20, 20, new List<Herbivore>(), new List<Carnivore>(), Message.CausationId,
                    Message.CorrelationId, Message.CreatedOn),
                new CreatePlantAccepted("plantId",2, 3,Message)
            };

            var when = new CreatePlants(2, 3, "plantId", Message.Id);

            Execute(given, when, typeof(Surface.PositionAlreadyTakenException), events => new Surface(events, new Random()));
        }
    }
}