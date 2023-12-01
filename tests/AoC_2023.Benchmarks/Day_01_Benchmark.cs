/*
 *
 *  | Method                             | Mean       | Error     | StdDev     | Median     | Ratio | RatioSD | Gen0     | Allocated | Alloc Ratio |
 *  |----------------------------------- |-----------:|----------:|-----------:|-----------:|------:|--------:|---------:|----------:|------------:|
 *  | Part1_Original                     | 336.745 us | 6.6735 us | 16.8649 us | 333.356 us |  1.00 |    0.00 | 125.0000 |  784416 B |        1.00 |
 *  | Part1_NoLinq                       | 155.492 us | 3.0939 us |  6.8560 us | 152.982 us |  0.46 |    0.03 |  59.0820 |  371952 B |        0.47 |
 *  | Part1_OptimizedTwoLoops            |  36.093 us | 0.5010 us |  0.4686 us |  35.908 us |  0.11 |    0.00 |        - |         - |        0.00 |
 *  | Part1                              |  36.399 us | 0.7115 us |  0.8470 us |  36.193 us |  0.11 |    0.01 |        - |         - |        0.00 |
 *  | Part1_CharIsDigit                  |  18.189 us | 0.3636 us |  0.3734 us |  18.035 us |  0.05 |    0.00 |        - |         - |        0.00 |
 *  | Part1_Span                         |   9.151 us | 0.1687 us |  0.1578 us |   9.184 us |  0.03 |    0.00 |        - |         - |        0.00 |
 *
 *  | Method                              | Mean     | Error     | StdDev    | Ratio | Gen0      | Gen1   | Allocated | Alloc Ratio |
 *  |------------------------------------ |---------:|----------:|----------:|------:|----------:|-------:|----------:|------------:|
 *  | Part2_Original                      | 9.846 ms | 0.1940 ms | 0.2156 ms |  1.00 | 1546.8750 |      - |   9.26 MB |        1.00 |
 *  | Part2                               | 3.469 ms | 0.0649 ms | 0.0638 ms |  0.35 |  398.4375 |      - |   2.39 MB |        0.26 |
 *  | Part2_ListPatternsAndSlidingWindows | 1.397 ms | 0.0271 ms | 0.0266 ms |  0.14 |  666.0156 | 1.9531 |   3.99 MB |        0.43 |
 *
*/

namespace AoC_2023.Benchmarks;

public class Day_01_Part1 : BaseDayBenchmark
{
    private readonly Day_01 _problem = new();

    [Benchmark(Baseline = true)]
    public int Part1_Original() => _problem.Solve_1_Original();

    [Benchmark]
    public int Part1_NoLinq() => _problem.Solve_1_NoLinq();

    [Benchmark]
    public int Part1_OptimizedTwoLoops() => _problem.Solve_1_NoLinq_OptimizedParsing();

    [Benchmark]
    public int Part1() => _problem.Solve_1_NoLinq_OptimizedParsing();

    [Benchmark]
    public int Part1_CharIsDigit() => _problem.Solve_1_CharIsDigit();

    [Benchmark]
    public int Part1_Span() => _problem.Solve_1_Span();
}

public class Day_01_Part2 : BaseDayBenchmark
{
    private readonly Day_01 _problem = new();

    [Benchmark(Baseline = true)]
    public int Part2_Original() => _problem.Solve_2_Original();

    [Benchmark]
    public int Part2() => _problem.Solve_2_Optimized();

    [Benchmark]
    public int Part2_ListPatternsAndSlidingWindows() => _problem.Solve_2_ListPatternsAndSlidingWindows();
}
