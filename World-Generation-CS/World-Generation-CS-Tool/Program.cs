using Microsoft.Extensions.Configuration;
using SixLabors.ImageSharp;
using World_Generation_CS;

IConfiguration configuration = new ConfigurationBuilder().AddCommandLine(args).Build();

int? randomSeed = configuration.GetValue<int?>("randomSeed");
int plotSize = configuration.GetValue<int?>("plotSize") ?? 10;
int numberOfCenters = configuration.GetValue<int?>("numberOfCenters") ?? 2000;
int width = configuration.GetValue<int?>("width") ?? 1920;
int height = configuration.GetValue<int?>("height") ?? 1080;

WorldGeneration worldGeneration = new WorldGeneration(plotSize, numberOfCenters, width, height, randomSeed);
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