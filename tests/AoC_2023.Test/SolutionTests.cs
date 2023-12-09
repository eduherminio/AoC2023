using AoCHelper;
using NUnit.Framework;

namespace AoC_2023.Test;

#pragma warning disable IL2067 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The parameter of method does not have matching annotations.
public static class SolutionTests
{
    [TestCase(typeof(Day_01), "54450", "54265")]
    [TestCase(typeof(Day_02), "2439", "63711")]
    [TestCase(typeof(Day_03), "507214", "72553319")]
    [TestCase(typeof(Day_04), "25004", "14427616")]
    [TestCase(typeof(Day_05), "551761867", "57451709", Explicit = true)]
    [TestCase(typeof(Day_06), "781200", "49240091")]
    [TestCase(typeof(Day_07), "246409899", "244848487")]
    [TestCase(typeof(Day_08), "18727", "18024643846273")]
    [TestCase(typeof(Day_09), "1882395907", "1005")]
    public static async Task Test(Type type, string sol1, string sol2)
    {
        if (Activator.CreateInstance(type) is BaseProblem instance)
        {
            await Assert.ThatAsync(async () => await instance.Solve_1(), Is.EqualTo(sol1));
            await Assert.ThatAsync(async () => await instance.Solve_2(), Is.EqualTo(sol2));
        }
        else
        {
            Assert.Fail($"{type} is not a BaseDay");
        }
    }
}
#pragma warning restore IL2067 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The parameter of method does not have matching annotations.
