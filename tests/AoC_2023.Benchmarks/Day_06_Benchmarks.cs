/*
 *
 *  | Method                               | pair                 | Mean              | Error           | StdDev          | Ratio | RatioSD | Allocated | Alloc Ratio |
 *  |------------------------------------- |--------------------- |------------------:|----------------:|----------------:|------:|--------:|----------:|------------:|
 *  | CountRecords                         | (94, 2047)           |        100.537 ns |       1.9287 ns |       1.8943 ns |  1.00 |    0.00 |         - |          NA |
 *  | CountRecordsWithBreak                | (94, 2047)           |         64.525 ns |       0.8310 ns |       0.7773 ns |  0.64 |    0.01 |         - |          NA |
 *  | CountRecordsWithBinarySearch         | (94, 2047)           |         35.435 ns |       0.6957 ns |       0.6167 ns |  0.35 |    0.01 |         - |          NA |
 *  | CountRecordsSolvingQuadraticEcuation | (94, 2047)           |          5.899 ns |       0.1138 ns |       0.1064 ns |  0.06 |    0.00 |         - |          NA |
 *
 *  | CountRecords                         | (6378(...)1035) [27] | 52,519,187.333 ns | 701,042.0074 ns | 655,755.1287 ns | 1.000 |    0.00 |      40 B |        1.00 |
 *  | CountRecordsWithBreak                | (6378(...)1035) [27] | 50,034,476.471 ns | 961,025.1824 ns | 986,902.1072 ns | 0.955 |    0.02 |      40 B |        1.00 |
 *  | CountRecordsWithBinarySearch         | (6378(...)1035) [27] | 49,907,165.385 ns | 897,225.0629 ns | 749,223.5630 ns | 0.949 |    0.02 |      40 B |        1.00 |
 *  | CountRecordsSolvingQuadraticEcuation | (6378(...)1035) [27] |          5.840 ns |       0.0789 ns |       0.0699 ns | 0.000 |    0.00 |         - |        0.00 |
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
    [Benchmark]
    [ArgumentsSource(nameof(Data))]
    public long CountRecordsSolvingQuadraticEcuation((int Time, int Distance) pair) => Day_06.CountRecordsSolvingQuadraticEcuation(pair.Time, pair.Distance);
}
