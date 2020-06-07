using System;
using NUnit.Framework;
using Sources.Photon;

namespace Sources.Common.Pattern.Editor
{
    public static class ColorPatternHelperTests
    {
        [Test]
        public static void ToFilledPattern()
        {
            int[] colors = {GameEventHelper.Blue, GameEventHelper.Green, GameEventHelper.Red, GameEventHelper.Yellow};
            int pattern = ColorPatternHelper.ToFilledPattern(colors);
            Assert.AreEqual(1, (pattern & 15));
            Assert.AreEqual(1 << 1, ((pattern >> (4 * 1)) & 15));
            Assert.AreEqual(1 << 2, ((pattern >> (4 * 2)) & 15));
            Assert.AreEqual(1 << 3, ((pattern >> (4 * 3)) & 15));
            
            pattern = Convert.ToInt32("1000010000100001", 2);
            Assert.AreEqual(1, (pattern & 15));
            Assert.AreEqual(1 << 1, ((pattern >> (4 * 1)) & 15));
            Assert.AreEqual(1 << 2, ((pattern >> (4 * 2)) & 15));
            Assert.AreEqual(1 << 3, ((pattern >> (4 * 3)) & 15));
        }
        
        [Test]
        public static void ToSinglePattern()
        {
            int[] colors = {GameEventHelper.Blue, GameEventHelper.Green, GameEventHelper.Red, GameEventHelper.Yellow};
            int filledPattern = ColorPatternHelper.ToFilledPattern(colors);
            Assert.AreEqual(GameEventHelper.Blue, ColorPatternHelper.ToSinglePattern(filledPattern, 0));
            Assert.AreEqual(GameEventHelper.Green, ColorPatternHelper.ToSinglePattern(filledPattern, 1));
            Assert.AreEqual(GameEventHelper.Red, ColorPatternHelper.ToSinglePattern(filledPattern, 2));
            Assert.AreEqual(GameEventHelper.Yellow, ColorPatternHelper.ToSinglePattern(filledPattern, 3));
        }
    }
}
