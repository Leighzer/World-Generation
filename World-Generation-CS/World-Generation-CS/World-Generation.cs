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
                    plots[i][j] = new Plot(new Vector2(plotSize * i, plotSize * j), plotSize, SharedUtils.color(0, 0, 0));
                }
            }
        }

        void initCenters()
        {
            for (int i = 0; i < centers.Length; i++)
            {
                centers[i] = new Center(new Vector2(SharedUtils.random(width), SharedUtils.random(height)), SharedUtils.randomBool(), getBiasedWarmLand());
            }

            for (int i = 0; i < centers.Length; i++)
            {
                if (!(centers[i].biome == Biome.OCEAN))
                {
                    centers[i].biome = Biome.OCEAN;
                    if (centers[i].pos.Y / height < (1 / (float)6) || centers[i].pos.Y / height > (5 / (float)6))
                    {
                        centers[i].biome = Biome.TUNDRA;
                    }
                    else
                    {
                        centers[i].biome = getRandomNotTundraOcean();
                    }
                }
            }
        }

        void updatePlot(Plot p)
        {
            float distance, closestDist;

            Center centerToUse;
            centerToUse = centers[0];
            closestDist = SharedUtils.dist(p.pos.X, p.pos.Y, centers[0].pos.X, centers[0].pos.Y);
            for (int i = 1; i < centers.Length; i++)
            {
                distance = SharedUtils.dist(p.pos.X, p.pos.Y, centers[i].pos.X, centers[i].pos.Y);
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
            image.Mutate(context => context.Fill(p.c, new Rectangle((int)p.pos.X, (int)p.pos.Y, p.size, p.size)));

            // if the plot has a structure
            if (p.s != null)
            {
                // render the structure
                // render p.s
                renderStructure(p.s);
            }
        }

        void renderStructure(Structure s)
        {
            var p = s.p;

            if (s.type == BuildingType.TREE)
            {
                image.Mutate(context => context.Fill(SharedUtils.color(139, 69, 19), new Rectangle((int)p.pos.X, (int)p.pos.Y, p.size, p.size)));
                image.Mutate(context => context.Fill(SharedUtils.color(139, 69, 19), new EllipsePolygon(new Point((int)p.pos.X, (int)p.pos.Y), p.size)));
            }
            else if (s.type == BuildingType.SNOW)
            {

            }
        }

        Biome getRandomBiome()
        {
            int randomVal = (int)Math.Floor(SharedUtils.random(4.9999f));
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
            int randomVal = (int)Math.Floor(SharedUtils.random(4.9999f));
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
            int randomVal = (int)Math.Floor(SharedUtils.random(2.9999f));
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
            int randomVal = (int)Math.Floor(SharedUtils.random(3.4999f));
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
