using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace AoC_2023.Test;
public class Day_10Tests
{
    public abstract class Day_10Test : Day_10
    {
        protected override string InputFileDirPath => "TestInputs";
    }

    public class Day_10Test_1: Day_10Test
    {
        public override string InputFilePath => Path.Combine(InputFileDirPath,
            "Day10_Part2_ExampleArea1.txt");
    }

    public class Day_10Test_2: Day_10Test
    {
        public override string InputFilePath => Path.Combine(InputFileDirPath,
            "Day10_Part2_ExampleArea2.txt");
    }

    public class Day_10Test_3: Day_10Test
    {
        public override string InputFilePath => Path.Combine(InputFileDirPath,
            "Day10_Part2_ExampleArea3.txt");
    }

    public class Day_10Test_4: Day_10Test
    {
        public override string InputFilePath => Path.Combine(InputFileDirPath,
            "Day10_Part2_ExampleArea4.txt");
    }

    [Test]
    public void Day10_Part2_Example1()
    {
        var day = new Day_10Test_1();
        ClassicAssert.AreEqual(4, day.CalculateArea());
    }

    [Test]
    public void Day10_Part2_Example2()
    {
        var day = new Day_10Test_2();
        ClassicAssert.AreEqual(4, day.CalculateArea());
    }

    [Test]
    public void Day10_Part2_Example3()
    {
        var day = new Day_10Test_3();
        ClassicAssert.AreEqual(8, day.CalculateArea());
    }

    [Test]
    public void Day10_Part2_Example4()
    {
        var day = new Day_10Test_4();
        ClassicAssert.AreEqual(10, day.CalculateArea());
    }
}
