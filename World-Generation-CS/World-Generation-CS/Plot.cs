using SkiaSharp;
using System.Numerics;

namespace World_Generation_CS
{
    public class Plot
    {
        public Vector2 Position { get; set; }
        public int Size { get; set; }
        public SKColor Color { get; set; }
        public Structure? Structure { get; set; }

        public Plot(Vector2 position, int size, SKColor color)
        {
            this.Position = position;
            this.Size = size;
            this.Color = color;
        }
    }
}
