using SixLabors.ImageSharp;
using System.Numerics;

namespace World_Generation_CS
{
    public class Plot
    {
        public Vector2 Position { get; set; }
        public int Size { get; set; }
        public Color Color { get; set; }
        public Structure Structure { get; set; }

        public Plot(Vector2 pos, int size, Color c)
        {
            this.Position = pos;
            this.Size = size;
            this.Color = c;
        }

        public void updateColor(Center centerToUse, float d)
        {
            //d = 1/d; 
            this.Structure = null;

            if (centerToUse.Biome == Biome.PLAINS)
            {
                if (d < 32)
                {
                    Color = SharedUtils.Color(153, 255, 51);
                }
                else
                {
                    Color = SharedUtils.Color(0, 255, 0);
                }
            }

            if (centerToUse.Biome == Biome.TUNDRA)
            {

                if (d < 16)
                {
                    Color = SharedUtils.Color(255, 255, 255);
                }
                else if (d < 32)
                {
                    Color = SharedUtils.Color(102, 178, 255);
                    this.Structure = new Structure(BuildingType.SNOW, this);
                }
                else
                {
                    Color = SharedUtils.Color(102, 178, 255);
                }

            }

            if (centerToUse.Biome == Biome.FOREST)
            {
                if (d < 32)
                {
                    Color = SharedUtils.Color(0, 133, 0);
                }
                else
                {
                    Color = SharedUtils.Color(0, 155, 0);
                }
                if (SharedUtils.Random(1) > 0.5)
                {
                    this.Structure = new Structure(BuildingType.TREE, this);
                }
            }

            if (centerToUse.Biome == Biome.OCEAN)
            {
                Color = SharedUtils.Color(0, 0, 255);
            }

            if (centerToUse.Biome == Biome.DESERT)
            {
                if (d < 32)
                {
                    Color = SharedUtils.Color(200, 180, 71);
                }
                else
                {
                    Color = SharedUtils.Color(229, 211, 101);
                }
            }
        }
    }
}
