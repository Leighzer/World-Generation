using SixLabors.ImageSharp;
using World_Generation_CS;

WorldGeneration worldGeneration = new WorldGeneration(10, 2000, 1920, 1080, 5);
worldGeneration.GenerateWorld();
Image image = worldGeneration.Render();
string filePath = "./" + Path.GetRandomFileName().Replace(".", "") + ".png";
image.SaveAsPng(filePath);

Console.WriteLine(Path.GetFullPath(filePath));
Console.WriteLine("DONE");

// to test if program is deterministic based on seeds
//worldGeneration.GenerateWorld();
//Image image2 = worldGeneration.Render();
//string filePath2 = "./" + Path.GetRandomFileName().Replace(".", "") + "_2.png";
//image2.SaveAsPng(filePath2);

//worldGeneration.SetSeed(6);
//worldGeneration.GenerateWorld();
//Image image3 = worldGeneration.Render();
//string filePath3 = "./" + Path.GetRandomFileName().Replace(".", "") + "_3.png";
//image3.SaveAsPng(filePath3);