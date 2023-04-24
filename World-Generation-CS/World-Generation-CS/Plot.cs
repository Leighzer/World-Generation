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
    }
}
