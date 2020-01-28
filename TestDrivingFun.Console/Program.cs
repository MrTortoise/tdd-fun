using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestDrivingFun.Engine;

namespace TestDrivingFun.Console
{
    class Program
    {
        static void Main(string[] _)
        {
            var sizeOfSurface = 20;
            var rnd = new Random(876);
            var coordinates = BuildUniqueCoordinates(20, rnd, sizeOfSurface);
            var surfaceEvents = new List<Event>()
            {
                new CreateBoardAccepted(sizeOfSurface, sizeOfSurface, coordinates.Take(10).Select(c=>new Herbivore(c.X,c.Y, "asd")), coordinates.Skip(10).Select(c=>new Carnivore(c.X,c.Y, "isaac", 10)), "none", "none", DateTime.Now)
            };
            var surface = new Surface(surfaceEvents, rnd);

            int numberOfHerbivores = 10;
            for (int i = 0; i < numberOfHerbivores; i++)
            {
                surface = AddHerbivore(surfaceEvents, rnd, sizeOfSurface, i);
            }

            int numberOfCarnivores = 10;
            for (int i = 0; i < numberOfCarnivores; i++)
            {
                surface = AddCarnivore(surfaceEvents, rnd, sizeOfSurface, i);
            }

            var toScreen = new SurfaceToConsoleAdapter();
            toScreen.Output(surface);


            StartSimulation(surfaceEvents, toScreen, rnd);
        }

        private static List<IHaveCoordinates> BuildUniqueCoordinates(int number, Random generator, int max)
        {
            if (number > max * max)
            {
                throw new ArgumentOutOfRangeException(nameof(number), "asked for more coords than spaces");
            }

            var retVal = new List<IHaveCoordinates>(number);
            while (number > 0)
            {
                var x = generator.Next(max);
                var y = generator.Next(max);
                if (!retVal.Exists(c => c.X == x && c.Y == y))
                {
                    retVal.Add(new Coordinate(x, y));
                    number--;
                }
            }

            return retVal;
        }

        private static void StartSimulation(List<Event> surfaceEvents, SurfaceToConsoleAdapter toScreen, Random rnd)
        {
            int currentMove = 0;
            while (true)
            {
                System.Console.Clear();
                System.Console.WriteLine($"Current Move is: {currentMove}");
                var surface = new Surface(surfaceEvents, rnd);
                toScreen.Output(surface);
                Task.Delay(TimeSpan.FromMilliseconds(500)).GetAwaiter().GetResult();
                currentMove++;
            }

        }

        private static Surface AddCarnivore(List<Event> surfaceEvents, Random rnd, int sizeOfSurface, int i)
        {
            Surface surface;
            surface = new Surface(surfaceEvents, rnd);
            var x = rnd.Next(sizeOfSurface);
            var y = rnd.Next(sizeOfSurface);
            try
            {
                var events = surface.Handle(new CreateCarnivore("testivore", x, y, 30, "createCarnivore-" + i));
                surfaceEvents.AddRange(events);
                return surface;
            }
            catch (Exception)
            {
                return AddCarnivore(surfaceEvents, rnd, sizeOfSurface, i);
            }
        }

        private static Surface AddHerbivore(List<Event> surfaceEvents, Random rnd, int sizeOfsurface, int i)
        {
            Surface surface;
            surface = new Surface(surfaceEvents, rnd);
            var x = rnd.Next(sizeOfsurface);
            var y = rnd.Next(sizeOfsurface);
            try
            {
                var events = surface.Handle(new CreateHerbivore("testivore", x, y, "createHerbivore-" + i));
                surfaceEvents.AddRange(events);
                return surface;
            }
            catch (Exception)
            {
                return AddHerbivore(surfaceEvents, rnd, sizeOfsurface, i);
            }
        }
    }

    internal class SurfaceToConsoleAdapter
    {
        public void Output(Surface surface)
        {
            var currentColour = System.Console.ForegroundColor;
            for (int j = 0; j < surface.Rows; j++)
            {
                for (int i = 0; i < surface.Columns; i++)
                {
                    switch (surface.Cells[i, j])
                    {
                        case Surface.CellType.Default:
                            {
                                System.Console.ForegroundColor = ConsoleColor.DarkGray;
                                System.Console.Write("#");
                                break;
                            }

                        case Surface.CellType.Herbivore:
                            {
                                System.Console.ForegroundColor = ConsoleColor.Green;
                                System.Console.Write("H");
                                break;
                            }

                        case Surface.CellType.Carnivore:
                            {
                                System.Console.ForegroundColor = ConsoleColor.Red;
                                System.Console.Write("C");
                                break;
                            }
                    }
                }
                System.Console.Write("\r\n");
            }

            System.Console.ForegroundColor = currentColour;
        }
    }
}
