using SixLabors.ImageSharp;

namespace World_Generation_CS
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WorldGeneration worldGeneration = new WorldGeneration();

            Image image = worldGeneration.Render();
            string filePath = "./" + Path.GetRandomFileName().Replace(".", "") + ".png";
            image.SaveAsPng(filePath);

            Console.WriteLine(Path.GetFullPath(filePath));
            Console.WriteLine("DONE");
        }
    }
}