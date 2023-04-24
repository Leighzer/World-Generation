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

        public Plot(Vector2 position, int size, Color color)
        {
            this.Position = position;
            this.Size = size;
            this.Color = color;
        }

        public void UpdateColor(Center centerToUse, float distance)
        {
            //d = 1/d; 
            this.Structure = null;

            if (centerToUse.Biome == Biome.PLAINS)
            {
                if (distance < 32)
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

                if (distance < 16)
                {
                    Color = SharedUtils.Color(255, 255, 255);
                }
                else if (distance < 32)
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
                if (distance < 32)
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
                if (distance < 32)
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
