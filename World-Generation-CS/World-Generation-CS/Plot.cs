using SixLabors.ImageSharp;
using System.Numerics;

namespace World_Generation_CS
{
    public class Plot
    {
        public Vector2 pos { get; set; }
        public int size { get; set; }
        public Color c { get; set; }
        public Structure s { get; set; }

        public Plot(Vector2 pos, int size, Color c)
        {
            this.pos = pos;
            this.size = size;
            this.c = c;
        }

        public void updateColor(Center centerToUse, float d)
        {
            //d = 1/d; 
            this.s = null;

            if (centerToUse.biome == Biome.PLAINS)
            {
                if (d < 32)
                {
                    c = SharedUtils.color(153, 255, 51);
                }
                else
                {
                    c = SharedUtils.color(0, 255, 0);
                }
            }

            if (centerToUse.biome == Biome.TUNDRA)
            {

                if (d < 16)
                {
                    c = SharedUtils.color(255, 255, 255);
                }
                else if (d < 32)
                {
                    c = SharedUtils.color(102, 178, 255);
                    this.s = new Structure(BuildingType.SNOW, this);
                }
                else
                {
                    c = SharedUtils.color(102, 178, 255);
                }

            }

            if (centerToUse.biome == Biome.FOREST)
            {
                if (d < 32)
                {
                    c = SharedUtils.color(0, 133, 0);
                }
                else
                {
                    c = SharedUtils.color(0, 155, 0);
                }
                if (SharedUtils.random(1) > 0.5)
                {
                    this.s = new Structure(BuildingType.TREE, this);
                }
            }

            if (centerToUse.biome == Biome.OCEAN)
            {
                c = SharedUtils.color(0, 0, 255);
            }

            if (centerToUse.biome == Biome.DESERT)
            {
                if (d < 32)
                {
                    c = SharedUtils.color(200, 180, 71);
                }
                else
                {
                    c = SharedUtils.color(229, 211, 101);
                }
            }
        }
    }
}
