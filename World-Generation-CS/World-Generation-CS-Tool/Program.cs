using SixLabors.ImageSharp;
using World_Generation_CS;

WorldGeneration worldGeneration = new WorldGeneration(10, 2000, 1920, 1080);
worldGeneration.GenerateWorld();
Image image = worldGeneration.Render();
string filePath = "./" + Path.GetRandomFileName().Replace(".", "") + ".png";
image.SaveAsPng(filePath);

Console.WriteLine(Path.GetFullPath(filePath));
Console.WriteLine("DONE");