using System;
using NUnit.Framework;
using Sources.Photon;

namespace Sources.Common.Pattern.Editor
{
    public static class ColorPatternHelperTests
    {
        [Test]
        public static void State()
        {
            int[] colors = {GameEventHelper.Blue, GameEventHelper.Green, GameEventHelper.Red, GameEventHelper.Yellow};
            int pattern = ColorPatternHelper.ToPattern(colors);
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
    }
}
