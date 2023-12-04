using AoCHelper;
using System.Reflection;

namespace AoC_2023.Benchmarks;

#pragma warning disable IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
#pragma warning disable IL2072 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
public class All_Days_Benchmark : BaseDayBenchmark
{
    private readonly List<Type>? _allDays = Assembly.GetAssembly(typeof(Day_01))
            ?.GetTypes()
            ?.Where(type => typeof(BaseProblem).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            ?.ToList();

    [Benchmark]
    public async Task SolveAll()
    {
        foreach (var type in _allDays ?? Enumerable.Empty<Type>())
        {
            var instance = Activator.CreateInstance(type) as BaseDay;

            await instance.Solve_1();
            await instance.Solve_2();
        }
    }
}
#pragma warning restore IL2026 // Members annotated with 'RequiresUnreferencedCodeAttribute' require dynamic access otherwise can break functionality when trimming application code
#pragma warning restore IL2072 // Target parameter argument does not satisfy 'DynamicallyAccessedMembersAttribute' in call to target method. The return value of the source method does not have matching annotations.
#pragma warning restore CS8602 // Dereference of a possibly null reference.