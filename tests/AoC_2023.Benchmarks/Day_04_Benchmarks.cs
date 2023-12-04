/*
 *
 *  | Method                        | Mean     | Error   | StdDev  | Ratio | RatioSD | Gen0    | Allocated | Alloc Ratio |
 *  |------------------------------ |---------:|--------:|--------:|------:|--------:|--------:|----------:|------------:|
 *  | Part1_Original                | 211.6 us | 4.18 us | 4.29 us |  1.00 |    0.00 | 49.0723 | 100.53 KB |        1.00 |
 *  | Part1_OptimizedIntersectOrder | 199.9 us | 3.98 us | 4.59 us |  0.95 |    0.03 | 79.1016 | 161.84 KB |        1.61 |
 *  | Part1                         | 187.9 us | 3.75 us | 3.85 us |  0.89 |    0.02 | 79.1016 |  161.8 KB |        1.61 |
 *
 *  | Method             | Mean     | Error   | StdDev   | Ratio | Gen0    | Allocated | Alloc Ratio |
 *  |------------------- |---------:|--------:|---------:|------:|--------:|----------:|------------:|
 *  | Part2_Original     | 430.6 us | 8.31 us | 12.44 us |  1.00 | 87.8906 | 180.45 KB |        1.00 |
 *  | Part2_NoDictionary | 186.5 us | 2.83 us |  2.64 us |  0.43 | 79.5898 | 162.67 KB |        0.90 |
 *
*/

namespace AoC_2023.Benchmarks;

public class Day_04_Part1 : BaseDayBenchmark
{
    private readonly Day_04 _problem = new();

    [Benchmark(Baseline = true)]
    public int Part1_Original() => _problem.Solve_1_Original();

    [Benchmark]
    public int Part1_OptimizedIntersectOrder() => _problem.Solve_1_OptimizedIntersectOrder();

    [Benchmark]
    public int Part1() => _problem.Solve_1_NoPow();
}

public class Day_04_Part2 : BaseDayBenchmark
{
    private readonly Day_04 _problem = new();

    [Benchmark(Baseline = true)]
    public int Part2_Original() => _problem.Solve_2_Original();

    [Benchmark]
    public int Part2_NoDictionary() => _problem.Solve_2_NoDictionary();
}
