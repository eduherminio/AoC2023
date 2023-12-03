/*
 *
 *  | Method         | Mean     | Error     | StdDev    | Median   | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
 *  |--------------- |---------:|----------:|----------:|---------:|------:|--------:|-------:|----------:|------------:|
 *  | Part1_Original | 2.466 us | 0.1180 us | 0.3479 us | 2.373 us |  1.00 |    0.00 | 0.0343 |      72 B |        1.00 |
 *  | Part1_NoLinq   | 1.250 us | 0.0185 us | 0.0254 us | 1.250 us |  0.56 |    0.06 |      - |         - |        0.00 |
 *
 *  | Method         | Mean      | Error     | StdDev    | Ratio | RatioSD | Gen0   | Allocated | Alloc Ratio |
 *  |--------------- |----------:|----------:|----------:|------:|--------:|-------:|----------:|------------:|
 *  | Part2_Original |  1.745 us | 0.0279 us | 0.0233 us |  1.00 |    0.00 |      - |         - |          NA |
 *  | Part2_Linq     | 27.601 us | 0.2838 us | 0.2516 us | 15.83 |    0.25 | 5.7373 |   12040 B |          NA |
 *
*/

namespace AoC_2023.Benchmarks;

public class Day_02_Part1 : BaseDayBenchmark
{
    private readonly Day_02 _problem = new();

    [Benchmark(Baseline = true)]
    public int Part1_Original() => _problem.Solve_1_Original();

    [Benchmark]
    public int Part1_NoLinq() => _problem.Solve_1_NoLinq();
}

public class Day_02_Part2 : BaseDayBenchmark
{
    private readonly Day_02 _problem = new();

    [Benchmark(Baseline = true)]
    public int Part2_Original() => _problem.Solve_2_Original();

    [Benchmark]
    public int Part2_Linq() => _problem.Solve_2_Linq();
}
