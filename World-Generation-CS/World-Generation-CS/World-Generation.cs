using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Numerics;

namespace World_Generation_CS
{
    public class WorldGeneration
    {
        public int PlotSize = 10;
        public int NumberOfRows;
        public int NumberOfColumns;
        public Plot[][] Plots;
        public Center[] Centers;
        public int NumberOfCenters = 2000;

        public const float Width = 1920;
        public const float Height = 1080;

        private Image<Rgba32> _image = new Image<Rgba32>((int)Width, (int)Height);

        private void Setup()
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

            UpdateColor(p, centerToUse, closestDist);
        }

        public void UpdateColor(Plot p, Center centerToUse, float distance)
        {
            //d = 1/d; 
            p.Structure = null;

            int coreDist0 = 20;
            int coreDist1 = 40;

            if (centerToUse.Biome == Biome.PLAINS)
            {
                if (distance < coreDist1)
                {
                    p.Color = SharedUtils.Color(153, 255, 51);
                }
                else
                {
                    p.Color = SharedUtils.Color(0, 255, 0);
                }
            }

            if (centerToUse.Biome == Biome.TUNDRA)
            {

                if (distance < coreDist0)
                {
                    p.Color = SharedUtils.Color(255, 255, 255);
                }
                else if (distance < coreDist1)
                {
                    p.Color = SharedUtils.Color(102, 178, 255);
                    p.Structure = new Structure(BuildingType.SNOW, p);
                }
                else
                {
                    p.Color = SharedUtils.Color(102, 178, 255);
                }

            }

            if (centerToUse.Biome == Biome.FOREST)
            {
                if (distance < coreDist1)
                {
                    p.Color = SharedUtils.Color(0, 133, 0);
                }
                else
                {
                    p.Color = SharedUtils.Color(0, 155, 0);
                }

                if (SharedUtils.Random(1) > 0.5)
                {
                    p.Structure = new Structure(BuildingType.TREE, p);
                }
            }

            if (centerToUse.Biome == Biome.OCEAN)
            {
                p.Color = SharedUtils.Color(0, 0, 255);
            }

            if (centerToUse.Biome == Biome.DESERT)
            {
                if (distance < coreDist1)
                {
                    p.Color = SharedUtils.Color(200, 180, 71);
                }
                else
                {
                    p.Color = SharedUtils.Color(229, 211, 101);
                }
            }
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
            _image.Mutate(context => context.Fill(p.Color, new Rectangle((int)p.Position.X, (int)p.Position.Y, p.Size, p.Size)));

            // if the plot has a structure
            if (p.Structure != null)
            {
                // render the structure
                // render p.s
                RenderStructure(p.Structure);
            }
        }

        private void RenderStructure(Structure structure)
        {
            var plot = structure.Plot;

            if (structure.BuildingType == BuildingType.TREE)
            {
                _image.Mutate(context => context.Fill(SharedUtils.Color(139, 69, 19), new Rectangle((int)plot.Position.X, (int)plot.Position.Y, plot.Size, plot.Size)));
                _image.Mutate(context => context.Fill(SharedUtils.Color(77, 255, 58), new EllipsePolygon(new Point((int)plot.Position.X + (plot.Size / 2), (int)plot.Position.Y + (plot.Size / 2)), plot.Size / 2)));
            }
            else if (structure.BuildingType == BuildingType.SNOW)
            {
                for (int i = 0; i < plot.Size; i++)
                {
                    for (int j = 0; j < plot.Size; j++)
                    {
                        int tx = (int)plot.Position.X + i;
                        int ty = (int)plot.Position.Y + j;
                        _image[tx, ty] = SharedUtils.RandomBool() ? Color.White : plot.Color;
                    }
                }
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
            Setup();
            return _image;
        }
    }
}
