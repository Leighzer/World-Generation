using System.Numerics;

namespace World_Generation_CS
{
    public class Center
    {
        public Vector2 Position { get; set; }
        public Biome Biome { get; set; }
        public bool IsPopulated { get; set; }

        public Center(Vector2 position, bool isPopulated, Biome biome)
        {
            this.Position = position;
            this.IsPopulated = isPopulated;
            this.Biome = biome;
        }
    }
}