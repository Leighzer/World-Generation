using System.Numerics;

namespace World_Generation_CS
{
    public class Center
    {
        public Vector2 pos { get; set; }
        public Biome biome { get; set; }
        public bool isPopulated { get; set; }

        public Center(Vector2 pos, bool isPopulated, Biome biome)
        {
            this.pos = pos;
            this.isPopulated = isPopulated;
            this.biome = biome;
        }
    }
}