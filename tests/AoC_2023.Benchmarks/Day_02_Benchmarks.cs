/*
 *
 *  | Method         | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
 *  |--------------- |----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
 *  | Part2_Original |  1.745 us | 0.0279 us | 0.0233 us |  1.00 |    0.00 |      - |         - |          NA |
 *  | Part2          | 27.601 us | 0.2838 us | 0.2516 us | 15.83 |    0.25 | 5.7373 |   12040 B |          NA |
 *
*/

namespace AoC_2023.Benchmarks;

public class Day_02_Part2 : BaseDayBenchmark
{
    private readonly Day_02 _problem = new();

    [Benchmark(Baseline = true)]
    public int Part2_Original() => _problem.Solve_2_Original();

    [Benchmark]
    public int Part2() => _problem.Solve_2_Linq();
}
