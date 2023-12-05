using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace AoC_2023.Test;
public class Day_05Tests
{
    [TestCase(50ul, 98ul, 2ul, 98ul, 50ul)]
    [TestCase(50ul, 98ul, 2ul, 99ul, 51ul)]
    [TestCase(50ul, 98ul, 2ul, 97ul, 97ul)]
    [TestCase(50ul, 98ul, 2ul, 100ul, 100ul)]

    [TestCase(52ul, 50ul, 48ul, 53ul, 55ul)]
    [TestCase(52ul, 50ul, 48ul, 49ul, 49ul)]
    [TestCase(52ul, 50ul, 48ul, 50ul, 52ul)]
    [TestCase(52ul, 50ul, 48ul, 51ul, 53ul)]
    [TestCase(52ul, 50ul, 48ul, 96ul, 98ul)]
    [TestCase(52ul, 50ul, 48ul, 97ul, 99ul)]
    public void Map_To(ulong destinationRangeStart, ulong sourceRangeStart, ulong rangeLength, ulong from, ulong expectedTo)
    {
        Map map = new(destinationRangeStart, sourceRangeStart, rangeLength);

        ClassicAssert.AreEqual(expectedTo, map.To(from));
    }
}
