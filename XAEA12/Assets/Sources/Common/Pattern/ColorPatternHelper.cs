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
    }
}
