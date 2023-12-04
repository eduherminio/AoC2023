/*
 *
 *  | Method             | Mean     | Error   | StdDev   | Ratio | Gen0    | Allocated | Alloc Ratio |
 *  |------------------- |---------:|--------:|---------:|------:|--------:|----------:|------------:|
 *  | Part2_Original     | 430.6 us | 8.31 us | 12.44 us |  1.00 | 87.8906 | 180.45 KB |        1.00 |
 *  | Part2_NoDictionary | 186.5 us | 2.83 us |  2.64 us |  0.43 | 79.5898 | 162.67 KB |        0.90 |
 *
*/

namespace AoC_2023.Benchmarks;

public class Day_04_Part2 : BaseDayBenchmark
{
    private readonly Day_04 _problem = new();

    [Benchmark(Baseline = true)]
    public int Part2_Original() => _problem.Solve_2_Original();

    [Benchmark]
    public int Part2_NoDictionary() => _problem.Solve_2_NoDictionary();
}
