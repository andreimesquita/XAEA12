using Sources.Photon;

namespace Sources.Common.Pattern
{
    public static class ColorPatternHelper
    {
        public static int ToPattern(int[] colorPatterns)
        {
            int pattern = 0;
            for (int i = 0; i < colorPatterns.Length; i++)
            {
                int color = colorPatterns[i];
                pattern |= color << (4 * i);
            }
            return pattern;
        }

        public static byte IndexToColorPattern(int index)
        {
            if (index == 0) return GameEventHelper.Blue;
            if (index == 1) return GameEventHelper.Green;
            if (index == 2) return GameEventHelper.Red;
            return GameEventHelper.Yellow;
        }

        public static int GetIndexByColorPattern(int colorPattern)
        {
            if (colorPattern == GameEventHelper.Blue) return 0;
            if (colorPattern == GameEventHelper.Green) return 1;
            if (colorPattern == GameEventHelper.Red) return 2;
            return 3;
        }
    }
}
