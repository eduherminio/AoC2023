/*
 *
 *  | Method | Mean     | Error     | StdDev    | Gen0     | Gen1    | Allocated |
 *  |------- |---------:|----------:|----------:|---------:|--------:|----------:|
 *  | Day_03 | 6.921 ms | 0.1345 ms | 0.1929 ms | 171.8750 | 93.7500 | 807.39 KB |
 *
*/

namespace AoC_2023.Benchmarks;

public class Day_03_Full : BaseDayBenchmark
{
    [Benchmark]
    public async Task Day_03()
    {
        var day3 = new Day_03();
        await day3.Solve_1();
        await day3.Solve_2();
    }
}
