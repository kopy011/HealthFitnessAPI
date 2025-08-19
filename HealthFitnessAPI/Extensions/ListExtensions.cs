namespace HealthFitnessAPI.Extensions;

public static class ListExtensions
{
    public static T GetRandom<T>(this List<T> list)
    {
        return list[new Random().Next(0, list.Count)];
    }

    public static List<T> Shuffle<T>(this List<T> list)
    {
        var rng = new Random();
        var n = list.Count;
        var shuffledList = new List<T>(list);

        for (var i = n - 1; i > 0; i--)
        {
            var j = rng.Next(i + 1);
            (shuffledList[i], shuffledList[j]) = (shuffledList[j], shuffledList[i]);
        }

        return shuffledList;
    }

    public static bool IsUnique<T>(this IEnumerable<T> source)
    {
        var seen = new HashSet<T>();
        return source.All(number => seen.Add(number));
    }
}