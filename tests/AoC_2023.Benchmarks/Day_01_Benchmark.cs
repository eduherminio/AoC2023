/*
 *
 *  | Method         | Mean      | Error    | StdDev   | Ratio | Gen0     | Allocated | Alloc Ratio |
 *  |--------------- |----------:|---------:|---------:|------:|---------:|----------:|------------:|
 *  | Part1          |  31.13 us | 0.604 us | 0.827 us |  0.10 |        - |         - |        0.00 |
 *  | Part1_NoLinq   | 146.30 us | 2.523 us | 2.236 us |  0.46 |  59.0820 |  371952 B |        0.47 |
 *  | Part1_Original | 320.12 us | 5.988 us | 6.149 us |  1.00 | 125.0000 |  784416 B |        1.00 |
 *
*/

namespace AoC_2023.Benchmarks;

public class Day_01_Part1 : BaseDayBenchmark
{
    private readonly Day_01 _problem = new();

    [Benchmark]
    public int Part1() =>  _problem.Solve_1_NoLinq_OptimizedParsing();

    [Benchmark]
    public int Part1_NoLinq() => _problem.Solve_1_NoLinq();

    [Benchmark(Baseline = true)]
    public int Part1_Original() => _problem.Solve_1_Original();
}

public class Day_01_Part2 : BaseDayBenchmark
{
    private readonly Day_01 _problem = new();

    [Benchmark]
    public int Part2() =>  _problem.Solve_2_Optimized();

    [Benchmark(Baseline = true)]
    public int Part2_Original() => _problem.Solve_2_Original();
}
