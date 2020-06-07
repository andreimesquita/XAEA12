using UnityEngine;

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

        public static Color ToColor(int colorPattern)
        {
            Color color = new Color();
            //TODO(andrei)
            return Color.black;
        }
    }
}
