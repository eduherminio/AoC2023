using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace AoC_2023.Test;
public class Day_05Tests
{
    [TestCase(50L, 98L, 2L, 98L, 50L)]
    [TestCase(50L, 98L, 2L, 99L, 51L)]
    [TestCase(50L, 98L, 2L, 97L, 97L)]
    [TestCase(50L, 98L, 2L, 100L, 100L)]

    [TestCase(52L, 50L, 48L, 53L, 55L)]
    [TestCase(52L, 50L, 48L, 49L, 49L)]
    [TestCase(52L, 50L, 48L, 50L, 52L)]
    [TestCase(52L, 50L, 48L, 51L, 53L)]
    [TestCase(52L, 50L, 48L, 96L, 98L)]
    [TestCase(52L, 50L, 48L, 97L, 99L)]
    public void Map_To(long destinationRangeStart, long sourceRangeStart, long rangeLength, long from, long expectedTo)
    {
        Map map = new(destinationRangeStart, sourceRangeStart, rangeLength);

        ClassicAssert.AreEqual(expectedTo, map.To(from));
    }
}
