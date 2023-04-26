using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Numerics;
using System.Security.Cryptography;

namespace World_Generation_CS
{
    public class WorldGeneration
    {
        private int _plotSize { get; set; }
        private int _numberOfCenters { get; set; }
        private int _width { get; set; }
        private int _height { get; set; }

        private int _numberOfRows { get; set; }
        private int _numberOfColumns { get; set; }
        private List<List<Plot>> _plots { get; set; }
        private List<Center> _centers { get; set; }
        private Image<Rgba32> _image { get; set; }

        private int _seed { get; set; }
        private Random _random { get; set; }

        public WorldGeneration(int plotSize, int numberOfCenters, int width, int height, int? seed = null)
        {
            InitializeSeedAndRng(seed);

            _plotSize = plotSize;
            _numberOfCenters = numberOfCenters;
            _width = width;
            _height = height;

            _numberOfRows = (int)Math.Floor((double)_width / _plotSize);
            _numberOfColumns = (int)Math.Floor((double)_height / _plotSize);
            _plots = new List<List<Plot>>();
            for (int i = 0; i < _numberOfRows; i++)
            {
                _plots.Add(new List<Plot>());
            }
            _centers = new List<Center>();
            _image = new Image<Rgba32>(_width, _height);
        }

        // null for a random seed
        public void SetSeed(int? seed)
        {
            InitializeSeedAndRng(seed);
        }

        private void InitializeSeedAndRng(int? seed)
        {
            if (seed == null)
            {
                byte[] bytes = new byte[4];
                RandomNumberGenerator.Fill(bytes);
                seed = BitConverter.ToInt32(bytes);
            }

            _seed = seed.Value;
            _random = new Random(_seed);
        }

        public void GenerateWorld()
        {
            // if we have generated before
            if (_centers.Any())
            {
                Clear();
                // reset rng back to start
                InitializeSeedAndRng(_seed);
            }

            InitializePlots();
            InitializeCenters();

            UpdatePlots();
        }

        private void Clear()
        {   
            _plots = new List<List<Plot>>();
            for (int i = 0; i < _numberOfRows; i++)
            {
                _plots.Add(new List<Plot>());
            }
            _centers = new List<Center>();
        }

        private void InitializePlots()
        {
            for (int i = 0; i < _numberOfRows; i++)
            {
                for (int j = 0; j < _numberOfColumns; j++)
                {
                    _plots[i].Add(new Plot(new Vector2(_plotSize * i, _plotSize * j), _plotSize, Color.FromRgb(0, 0, 0)));
                }
            }
        }

        private void InitializeCenters()
        {
            for (int i = 0; i < _numberOfCenters; i++)
            {
                _centers.Add(new Center(new Vector2(Random(_width), Random(_height)), RandomBool(), GetBiasedWarmLand()));
            }

            for (int i = 0; i < _centers.Count; i++)
            {
                if (!(_centers[i].Biome == Biome.OCEAN))
                {
                    _centers[i].Biome = Biome.OCEAN;
                    if (_centers[i].Position.Y / _height < (1 / (float)6) || _centers[i].Position.Y / _height > (5 / (float)6))
                    {
                        _centers[i].Biome = Biome.TUNDRA;
                    }
                    else
                    {
                        _centers[i].Biome = GetRandomNotTundraOcean();
                    }
                }
            }
        }

        private void UpdatePlot(Plot p)
        {
            float distance, closestDist;

            Center centerToUse;
            centerToUse = _centers[0];
            closestDist = Distance(p.Position.X, p.Position.Y, _centers[0].Position.X, _centers[0].Position.Y);
            for (int i = 1; i < _centers.Count; i++)
            {
                distance = Distance(p.Position.X, p.Position.Y, _centers[i].Position.X, _centers[i].Position.Y);
                if (distance < closestDist)
                {
                    closestDist = distance;
                    centerToUse = _centers[i];
                }
            }

            UpdateColor(p, centerToUse, closestDist);
        }

        private void UpdateColor(Plot p, Center centerToUse, float distance)
        {
            //d = 1/d; 
            p.Structure = null;

            int coreDist0 = 20;
            int coreDist1 = 40;

            if (centerToUse.Biome == Biome.PLAINS)
            {
                if (distance < coreDist1)
                {
                    p.Color = Color.FromRgb(153, 255, 51);
                }
                else
                {
                    p.Color = Color.FromRgb(0, 255, 0);
                }
            }

            if (centerToUse.Biome == Biome.TUNDRA)
            {

                if (distance < coreDist0)
                {
                    p.Color = Color.FromRgb(255, 255, 255);
                }
                else if (distance < coreDist1)
                {
                    p.Color = Color.FromRgb(102, 178, 255);
                    p.Structure = new Structure(BuildingType.SNOW, p);
                }
                else
                {
                    p.Color = Color.FromRgb(102, 178, 255);
                }

            }

            if (centerToUse.Biome == Biome.FOREST)
            {
                if (distance < coreDist1)
                {
                    p.Color = Color.FromRgb(0, 133, 0);
                }
                else
                {
                    p.Color = Color.FromRgb(0, 155, 0);
                }

                if (Random(1) > 0.5)
                {
                    p.Structure = new Structure(BuildingType.TREE, p);
                }
            }

            if (centerToUse.Biome == Biome.OCEAN)
            {
                p.Color = Color.FromRgb(0, 0, 255);
            }

            if (centerToUse.Biome == Biome.DESERT)
            {
                if (distance < coreDist1)
                {
                    p.Color = Color.FromRgb(200, 180, 71);
                }
                else
                {
                    p.Color = Color.FromRgb(229, 211, 101);
                }
            }
        }

        private void UpdatePlots()
        {
            for (int i = 0; i < _numberOfRows; i++)
            {
                for (int j = 0; j < _numberOfColumns; j++)
                {
                    UpdatePlot(_plots[i][j]);
                }
            }
        }

        private void RenderPlots()
        {
            for (int i = 0; i < _numberOfRows; i++)
            {
                for (int j = 0; j < _numberOfColumns; j++)
                {
                    RenderPlot(_plots[i][j]);
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
                _image.Mutate(context => context.Fill(Color.FromRgb(139, 69, 19), new Rectangle((int)plot.Position.X, (int)plot.Position.Y, plot.Size, plot.Size)));
                _image.Mutate(context => context.Fill(Color.FromRgb(77, 255, 58), new EllipsePolygon(new Point((int)plot.Position.X + (plot.Size / 2), (int)plot.Position.Y + (plot.Size / 2)), plot.Size / 2)));
            }
            else if (structure.BuildingType == BuildingType.SNOW)
            {
                for (int i = 0; i < plot.Size; i++)
                {
                    for (int j = 0; j < plot.Size; j++)
                    {
                        int tx = (int)plot.Position.X + i;
                        int ty = (int)plot.Position.Y + j;
                        _image[tx, ty] = RandomBool() ? Color.White : plot.Color;
                    }
                }
            }
        }

        private Biome GetRandomBiome()
        {
            int randomVal = (int)Math.Floor(Random(4.9999f));
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
            int randomVal = (int)Math.Floor(Random(4.9999f));
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
            int randomVal = (int)Math.Floor(Random(2.9999f));
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
            int randomVal = (int)Math.Floor(Random(3.4999f));
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

        // render world state to Image and return it
        public Image Render()
        {
            RenderPlots();
            return _image;
        }

        private float Random(float input)
        {   
            return (float)(_random.NextDouble() * input);
        }

        private bool RandomBool()
        {
            return Random(1) > 0.5;
        }

        private int RandomSign()
        {
            if (RandomBool())
            {
                return 1;
            }
            else
            {
                return -1;
            }
        }

        private float Distance(float x1, float y1, float x2, float y2)
        {
            float dist = (float)Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
            return dist;
        }
    }
}
