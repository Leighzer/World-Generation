using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Numerics;

// import toxi.math.noise.SimplexNoise;

namespace World_Generation_CS
{
    public class WorldGeneration
    {
        int plotSize = 10;
        int numberOfRows;
        int numberOfColumns;
        Plot[][] plots;
        Center[] centers;
        int numberOfCenters = 2500;

        const float width = 640;
        const float height = 360;

        Image<Rgba32> image = new Image<Rgba32>((int)width, (int)height);

        public void setup()
        {
            numberOfRows = (int)Math.Floor(width / plotSize);
            numberOfColumns = (int)Math.Floor(height / plotSize);
            plots = new Plot[numberOfRows][];
            for (int i = 0; i < numberOfRows; i++)
            {
                plots[i] = new Plot[numberOfColumns];
            }
            centers = new Center[numberOfCenters];

            initPlots();
            initCenters();

            updatePlots();
            drawPlots();
        }

        void initPlots()
        {
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    plots[i][j] = new Plot(new Vector2(plotSize * i, plotSize * j), plotSize, SharedUtils.Color(0, 0, 0));
                }
            }
        }

        void initCenters()
        {
            for (int i = 0; i < centers.Length; i++)
            {
                centers[i] = new Center(new Vector2(SharedUtils.Random(width), SharedUtils.Random(height)), SharedUtils.RandomBool(), getBiasedWarmLand());
            }

            for (int i = 0; i < centers.Length; i++)
            {
                if (!(centers[i].Biome == Biome.OCEAN))
                {
                    centers[i].Biome = Biome.OCEAN;
                    if (centers[i].Position.Y / height < (1 / (float)6) || centers[i].Position.Y / height > (5 / (float)6))
                    {
                        centers[i].Biome = Biome.TUNDRA;
                    }
                    else
                    {
                        centers[i].Biome = getRandomNotTundraOcean();
                    }
                }
            }
        }

        void updatePlot(Plot p)
        {
            float distance, closestDist;

            Center centerToUse;
            centerToUse = centers[0];
            closestDist = SharedUtils.Distance(p.Position.X, p.Position.Y, centers[0].Position.X, centers[0].Position.Y);
            for (int i = 1; i < centers.Length; i++)
            {
                distance = SharedUtils.Distance(p.Position.X, p.Position.Y, centers[i].Position.X, centers[i].Position.Y);
                if (distance < closestDist)
                {
                    closestDist = distance;
                    centerToUse = centers[i];
                }
            }

            p.updateColor(centerToUse, closestDist);
        }

        void updatePlots()
        {
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    updatePlot(plots[i][j]);
                }
            }
        }

        void drawPlots()
        {
            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfColumns; j++)
                {
                    renderPlot(plots[i][j]);
                }
            }
        }

        void renderPlot(Plot p)
        {
            // render the plot bkd
            image.Mutate(context => context.Fill(p.Color, new Rectangle((int)p.Position.X, (int)p.Position.Y, p.Size, p.Size)));

            // if the plot has a structure
            if (p.Structure != null)
            {
                // render the structure
                // render p.s
                renderStructure(p.Structure);
            }
        }

        void renderStructure(Structure s)
        {
            var p = s.Plot;

            if (s.BuildingType == BuildingType.TREE)
            {
                image.Mutate(context => context.Fill(SharedUtils.Color(139, 69, 19), new Rectangle((int)p.Position.X, (int)p.Position.Y, p.Size, p.Size)));
                image.Mutate(context => context.Fill(SharedUtils.Color(139, 69, 19), new EllipsePolygon(new Point((int)p.Position.X, (int)p.Position.Y), p.Size)));
            }
            else if (s.BuildingType == BuildingType.SNOW)
            {

            }
        }

        Biome getRandomBiome()
        {
            int randomVal = (int)Math.Floor(SharedUtils.Random(4.9999f));
            if (randomVal == 0)
            {
                return Biome.FOREST;
            }
            else if (randomVal == 1)
            {
                return Biome.PLAINS;
            }
            else if (randomVal == 2)
            {
                return Biome.TUNDRA;
            }
            else if (randomVal == 3)
            {
                return Biome.OCEAN;
            }
            else
            {//(randomVal == 4)
                return Biome.DESERT;
            }
        }

        Biome getRandomNotTundra()
        {
            int randomVal = (int)Math.Floor(SharedUtils.Random(4.9999f));
            if (randomVal == 0)
            {
                return Biome.FOREST;
            }
            else if (randomVal == 1)
            {
                return Biome.PLAINS;
            }
            else if (randomVal == 3)
            {
                return Biome.OCEAN;
            }
            else
            {//(randomVal == 4)
                return Biome.DESERT;
            }
        }

        Biome getRandomNotTundraOcean()
        {
            int randomVal = (int)Math.Floor(SharedUtils.Random(2.9999f));
            if (randomVal == 0)
            {
                return Biome.FOREST;
            }
            else if (randomVal == 1)
            {
                return Biome.PLAINS;
            }
            else
            {//(randomVal == 2)
                return Biome.DESERT;
            }
        }

        Biome getBiasedWarmLand()
        {
            int randomVal = (int)Math.Floor(SharedUtils.Random(3.4999f));
            if (randomVal == 0)
            {
                return Biome.FOREST;
            }
            else if (randomVal == 1)
            {
                return Biome.PLAINS;
            }
            else if (randomVal == 2)
            {
                return Biome.DESERT;
            }
            else
            {//(randomVal == 3)
                return Biome.OCEAN;
            }
        }

        public Image render()
        {
            // TODO return image
            return null;
        }
    }
}
