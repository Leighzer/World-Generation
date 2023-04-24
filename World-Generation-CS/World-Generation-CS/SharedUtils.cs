using SixLabors.ImageSharp;

namespace World_Generation_CS
{
    // util file that provides similar functionality to processing shared functions
    // temporary
    public static class SharedUtils
    {
        public static float Random(float input)
        {
            Random random = new Random();
            return (float)(random.NextDouble() * input);
        }

        public static Color Color(int r, int g, int b)
        {
            return SixLabors.ImageSharp.Color.FromRgb((byte)r, (byte)g, (byte)b);
        }

        public static float Distance(float x1, float y1, float x2, float y2)
        {
            float dist = (float)Math.Sqrt(Math.Pow((x2 - x1), 2) + Math.Pow((y2 - y1), 2));
            return dist;
        }

        public static bool RandomBool()
        {
            return Random(1) > 0.5;
        }

        public static int RandomSign()
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
    }
}
