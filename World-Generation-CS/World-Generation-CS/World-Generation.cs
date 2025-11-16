using SkiaSharp;
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
        private SKBitmap _bitmap { get; set; }

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
            _bitmap = new SKBitmap(_width, _height, SKColorType.Rgba8888, SKAlphaType.Premul);
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
                    _plots[i].Add(new Plot(new Vector2(_plotSize * i, _plotSize * j), _plotSize, new SKColor(0, 0, 0)));
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
                    p.Color = new SKColor(153, 255, 51);
                }
                else
                {
                    p.Color = new SKColor(0, 255, 0);
                }
            }

            if (centerToUse.Biome == Biome.TUNDRA)
            {

                if (distance < coreDist0)
                {
                    p.Color = new SKColor(255, 255, 255);
                }
                else if (distance < coreDist1)
                {
                    p.Color = new SKColor(102, 178, 255);
                    p.Structure = new Structure(BuildingType.SNOW, p);
                }
                else
                {
                    p.Color = new SKColor(102, 178, 255);
                }

            }

            if (centerToUse.Biome == Biome.FOREST)
            {
                if (distance < coreDist1)
                {
                    p.Color = new SKColor(0, 133, 0);
                }
                else
                {
                    p.Color = new SKColor(0, 155, 0);
                }

                if (Random(1) > 0.5)
                {
                    p.Structure = new Structure(BuildingType.TREE, p);
                }
            }

            if (centerToUse.Biome == Biome.OCEAN)
            {
                p.Color = new SKColor(0, 0, 255);
            }

            if (centerToUse.Biome == Biome.DESERT)
            {
                if (distance < coreDist1)
                {
                    p.Color = new SKColor(200, 180, 71);
                }
                else
                {
                    p.Color = new SKColor(229, 211, 101);
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
            using var canvas = new SKCanvas(_bitmap);   // ✔ only once

            canvas.Clear(SKColors.Black);

            for (int i = 0; i < _numberOfRows; i++)
            {
                for (int j = 0; j < _numberOfColumns; j++)
                {
                    RenderPlot(_plots[i][j], canvas);
                }
            }
        }

        private void RenderPlot(Plot p, SKCanvas canvas)
        {
            // render the plot bkd
            using var paint = new SKPaint
            {
                Color = p.Color,
                Style = SKPaintStyle.Fill,
                IsAntialias = false
            };
            var rect = new SKRect(
                p.Position.X,
                p.Position.Y,
                p.Position.X + p.Size,
                p.Position.Y + p.Size
            );
            canvas.DrawRect(rect, paint);

            // if the plot has a structure
            if (p.Structure != null)
            {
                // render the structure
                // render p.s
                RenderStructure(p.Structure, canvas);
            }
        }

        private void RenderStructure(Structure structure, SKCanvas canvas)
        {
            var plot = structure.Plot;

            if (structure.BuildingType == BuildingType.TREE)
            {
                using var paint = new SKPaint
                {
                    Color = new SKColor(139, 69, 19),
                    Style = SKPaintStyle.Fill,
                    IsAntialias = false
                };
                var rect = new SKRect(
                    (float)plot.Position.X,
                    (float)plot.Position.Y,
                    (float)plot.Position.X + plot.Size,
                    (float)plot.Position.Y + plot.Size
                );
                canvas.DrawRect(rect, paint);

                using var paint2 = new SKPaint
                {
                    Color = new SKColor(77, 255, 58),
                    Style = SKPaintStyle.Fill,
                    IsAntialias = true
                };
                float centerX = (float)plot.Position.X + (plot.Size / 2f);
                float centerY = (float)plot.Position.Y + (plot.Size / 2f);
                float radius = plot.Size / 2f;
                canvas.DrawCircle(centerX, centerY, radius, paint2);
            }
            else if (structure.BuildingType == BuildingType.SNOW)
            {
                for (int i = 0; i < plot.Size; i++)
                {
                    for (int j = 0; j < plot.Size; j++)
                    {
                        int tx = (int)plot.Position.X + i;
                        int ty = (int)plot.Position.Y + j;
                        var randomColor = RandomBool() ? SKColors.White : plot.Color;
                        _bitmap.SetPixel(tx, ty, randomColor);
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
        public SKBitmap Render()
        {
            RenderPlots();
            return _bitmap;
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
