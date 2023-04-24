﻿using SixLabors.ImageSharp;
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
        public int PlotSize = 10;
        public int NumberOfRows;
        public int NumberOfColumns;
        public Plot[][] Plots;
        public Center[] Centers;
        public int NumberOfCenters = 2500;

        public const float Width = 640;
        public const float Height = 360;

        public Image<Rgba32> Image = new Image<Rgba32>((int)Width, (int)Height);

        public void Setup()
        {
            NumberOfRows = (int)Math.Floor(Width / PlotSize);
            NumberOfColumns = (int)Math.Floor(Height / PlotSize);
            Plots = new Plot[NumberOfRows][];
            for (int i = 0; i < NumberOfRows; i++)
            {
                Plots[i] = new Plot[NumberOfColumns];
            }
            Centers = new Center[NumberOfCenters];

            InitializePlots();
            InitializeCenters();

            UpdatePlots();
            RenderPlots();
        }

        private void InitializePlots()
        {
            for (int i = 0; i < NumberOfRows; i++)
            {
                for (int j = 0; j < NumberOfColumns; j++)
                {
                    Plots[i][j] = new Plot(new Vector2(PlotSize * i, PlotSize * j), PlotSize, SharedUtils.Color(0, 0, 0));
                }
            }
        }

        private void InitializeCenters()
        {
            for (int i = 0; i < Centers.Length; i++)
            {
                Centers[i] = new Center(new Vector2(SharedUtils.Random(Width), SharedUtils.Random(Height)), SharedUtils.RandomBool(), GetBiasedWarmLand());
            }

            for (int i = 0; i < Centers.Length; i++)
            {
                if (!(Centers[i].Biome == Biome.OCEAN))
                {
                    Centers[i].Biome = Biome.OCEAN;
                    if (Centers[i].Position.Y / Height < (1 / (float)6) || Centers[i].Position.Y / Height > (5 / (float)6))
                    {
                        Centers[i].Biome = Biome.TUNDRA;
                    }
                    else
                    {
                        Centers[i].Biome = GetRandomNotTundraOcean();
                    }
                }
            }
        }

        private void UpdatePlot(Plot p)
        {
            float distance, closestDist;

            Center centerToUse;
            centerToUse = Centers[0];
            closestDist = SharedUtils.Distance(p.Position.X, p.Position.Y, Centers[0].Position.X, Centers[0].Position.Y);
            for (int i = 1; i < Centers.Length; i++)
            {
                distance = SharedUtils.Distance(p.Position.X, p.Position.Y, Centers[i].Position.X, Centers[i].Position.Y);
                if (distance < closestDist)
                {
                    closestDist = distance;
                    centerToUse = Centers[i];
                }
            }

            p.UpdateColor(centerToUse, closestDist);
        }

        private void UpdatePlots()
        {
            for (int i = 0; i < NumberOfRows; i++)
            {
                for (int j = 0; j < NumberOfColumns; j++)
                {
                    UpdatePlot(Plots[i][j]);
                }
            }
        }

        private void RenderPlots()
        {
            for (int i = 0; i < NumberOfRows; i++)
            {
                for (int j = 0; j < NumberOfColumns; j++)
                {
                    RenderPlot(Plots[i][j]);
                }
            }
        }

        private void RenderPlot(Plot p)
        {
            // render the plot bkd
            Image.Mutate(context => context.Fill(p.Color, new Rectangle((int)p.Position.X, (int)p.Position.Y, p.Size, p.Size)));

            // if the plot has a structure
            if (p.Structure != null)
            {
                // render the structure
                // render p.s
                RenderStructure(p.Structure);
            }
        }

        private void RenderStructure(Structure s)
        {
            var p = s.Plot;

            if (s.BuildingType == BuildingType.TREE)
            {
                Image.Mutate(context => context.Fill(SharedUtils.Color(139, 69, 19), new Rectangle((int)p.Position.X, (int)p.Position.Y, p.Size, p.Size)));
                Image.Mutate(context => context.Fill(SharedUtils.Color(139, 69, 19), new EllipsePolygon(new Point((int)p.Position.X, (int)p.Position.Y), p.Size)));
            }
            else if (s.BuildingType == BuildingType.SNOW)
            {

            }
        }

        private Biome GetRandomBiome()
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

        private Biome GetRandomNotTundra()
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

        private Biome GetRandomNotTundraOcean()
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

        private Biome GetBiasedWarmLand()
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

        public Image Render()
        {
            return Image;
        }
    }
}
