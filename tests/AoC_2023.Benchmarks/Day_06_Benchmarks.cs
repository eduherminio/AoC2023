/*
 *
 *  | Method                       | pair                 | Mean             | Error          | StdDev         | Ratio | RatioSD | Allocated | Alloc Ratio |
 *  |----------------------------- |--------------------- |-----------------:|---------------:|---------------:|------:|--------:|----------:|------------:|
 *  | CountRecords                 | (94, 2047)           |         97.99 ns |       1.694 ns |       1.584 ns |  1.00 |    0.00 |         - |          NA |
 *  | CountRecordsWithBreak        | (94, 2047)           |         64.78 ns |       1.302 ns |       1.393 ns |  0.66 |    0.02 |         - |          NA |
 *  | CountRecordsWithBinarySearch | (94, 2047)           |         35.11 ns |       0.216 ns |       0.202 ns |  0.36 |    0.01 |         - |          NA |
 *
 *  | CountRecords                 | (6378(...)1035) [27] | 51,650,090.67 ns | 329,428.606 ns | 308,147.722 ns |  1.00 |    0.00 |      40 B |        1.00 |
 *  | CountRecordsWithBreak        | (6378(...)1035) [27] | 48,991,428.57 ns | 467,502.797 ns | 414,429.053 ns |  0.95 |    0.01 |      40 B |        1.00 |
 *  | CountRecordsWithBinarySearch | (6378(...)1035) [27] | 48,620,250.37 ns | 228,947.966 ns | 214,158.070 ns |  0.94 |    0.01 |      44 B |        1.10 |
 *
*/

namespace AoC_2023.Benchmarks;

public class Day_06_Benchmark : BaseDayBenchmark
{
    public static (int, long)[] Data => [(94, 2047), (63_78_94_68, 411_1274_2047_1035L)];

    [Benchmark(Baseline = true)]
    [ArgumentsSource(nameof(Data))]

    public long CountRecords((int Time, long Distance) pair) => Day_06.CountRecords(pair.Time, pair.Distance);

    [Benchmark]
    [ArgumentsSource(nameof(Data))]

    public long CountRecordsWithBreak((int Time, int Distance) pair) => Day_06.CountRecordsWithEarlyBreak(pair.Time, pair.Distance);

    [Benchmark]
    [ArgumentsSource(nameof(Data))]

    public long CountRecordsWithBinarySearch((int Time, int Distance) pair) => Day_06.CountRecordsWithBinarySearch(pair.Time, pair.Distance);
}
