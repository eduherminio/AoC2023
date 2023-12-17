using SheepTools.Model;

namespace AoC_2023;
internal static class Helpers
{
    public static void PrintGrid<T>(this IEnumerable<IEnumerable<IntPointWithValue<T>>> grid)
    {
        for (int y = 0; y < grid.Count(); ++y)
        {
            for (int x = 0; x < grid.ElementAt(y).Count(); ++x)
            {
                Console.Write(grid.ElementAt(y).ElementAt(x).Value);
            }
            Console.WriteLine();
        }
    }
}
