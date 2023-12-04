using AoCHelper;
using System.Reflection;

namespace AoC_2023.Benchmarks;

#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
#pragma warning disable IL2067 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The parameter of method does not have matching annotations.
#pragma warning disable S2365 // Properties should not make collection or array copies
public class All_Days_Detailed_Benchmark : BaseDayBenchmark
{
    public static List<Type>? AllDayTypes => Assembly.GetAssembly(typeof(Day_01))
            ?.GetTypes()
            ?.Where(type => typeof(BaseProblem).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            ?.ToList();

    public static List<BaseDay?>? AllDays => Assembly.GetAssembly(typeof(Day_01))
            ?.GetTypes()
            ?.Where(type => typeof(BaseDay).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            ?.Select(type => Activator.CreateInstance(type) as BaseDay)
            .ToList();

    [Benchmark]
    [ArgumentsSource(nameof(AllDayTypes))]
    public object? Constructor(Type day)
    {
        return Activator.CreateInstance(day) as BaseDay;
    }

    [Benchmark]
    [ArgumentsSource(nameof(AllDays))]
    public async ValueTask<string> Part1(BaseDay day)
    {
        return await day.Solve_1();
    }

    [Benchmark]
    [ArgumentsSource(nameof(AllDays))]
    public async ValueTask<string> Part2(BaseDay day)
    {
        return await day.Solve_2();
    }
}
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
#pragma warning restore IL2067 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The parameter of method does not have matching annotations.
#pragma warning restore S2365 // Properties should not make collection or array copies