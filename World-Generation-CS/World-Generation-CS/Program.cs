using SixLabors.ImageSharp;

namespace World_Generation_CS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WorldGeneration worldGeneration = new WorldGeneration();

            worldGeneration.Setup();

            Image worldImage = worldGeneration.Render();

        }
    }
}