using AoCHelper;
using System.Reflection;

namespace AoC_2023.Benchmarks;

#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
#pragma warning disable IL2067 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The parameter of method does not have matching annotations.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
#pragma warning disable S2365 // Properties should not make collection or array copies
public class All_Days_Individually_Benchmark : BaseDayBenchmark
{
    public static List<Type>? AllDays => Assembly.GetAssembly(typeof(Day_01))
            ?.GetTypes()
            ?.Where(type => typeof(BaseProblem).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            ?.ToList();

    [Benchmark]
    [ArgumentsSource(nameof(AllDays))]
    public async Task Solve(Type type)
    {
        var instance = Activator.CreateInstance(type) as BaseDay;

        await instance.Solve_1();
        await instance.Solve_2();
    }
}
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
#pragma warning restore IL2067 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The parameter of method does not have matching annotations.
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore S2365 // Properties should not make collection or array copies