using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class EnumerableExtensions
{
    public static T Random<T>(this IList<T> @this)
    {
        var count = @this.Count;
        if (count == 0) return default;
        var index = UnityEngine.Random.Range(0, count);
        return @this[index];
    }

    public static IEnumerable<T> GetRandomElements<T>(this IList<T> @this, int count)
    {
        var copyList = new List<T>(@this);
        for (int i = 0; i < count; i++)
        {
            if (copyList.Count == 0) yield break;
            var element = copyList.Random();
            yield return element;
            copyList.Remove(element);
        }
    }

    public static KeyValuePair<TKey, TValue> Random<TKey, TValue>(this IDictionary<TKey, TValue> @this)
    {
        var count = @this.Count;
        if (count == 0) return default;
        var index = UnityEngine.Random.Range(0, count);
        using (var enumerator = @this.GetEnumerator())
        {
            for (int i = 0; i <= index; i++)
            {
                enumerator.MoveNext();
            }
            return enumerator.Current;
        }
    }

    public static List<T> Random<T>(this ICollection<T> @this, int count)
    {
        var result = new List<T>();
        if (@this == null || @this.Count == 0 || count <= 0) return result;

        var availableItems = new List<T>(@this);
        var random = new System.Random();
        var amountToTake = count >= @this.Count ? @this.Count : count;

        for (int i = 0; i < count && availableItems.Count > 0; i++)
        {
            var index = random.Next(availableItems.Count);
            result.Add(availableItems[index]);
            availableItems.RemoveAt(index);
        }

        return result;
    }

    public static List<List<T>> Split<T>(this List<T> source, int numberOfChunks)
    {
        var result = new List<List<T>>();
        if (numberOfChunks <= 0 || source == null || source.Count == 0)
            return result;

        int chunkSize = source.Count / numberOfChunks;
        int remainder = source.Count % numberOfChunks;
        int index = 0;

        for (int i = 0; i < numberOfChunks; i++)
        {
            int currentChunkSize = chunkSize + (remainder > 0 ? 1 : 0); // Distribute remaining items
            result.Add(source.GetRange(index, currentChunkSize));
            index += currentChunkSize;
            remainder--;
        }

        return result;
    }

    public static IList<T> Shuffle<T>(this IList<T> @this)
    {
        var count = @this.Count;
        var c = count;
        var arr = new int[count];
        var taken = new bool[count];
        for (int i = 0; i < count; i++)
        {
            var rand = UnityEngine.Random.Range(0, c--) + 1;
            var index = -1;
            while (rand > 0)
            {
                if (!taken[++index])
                    rand--;
            }
            arr[i] = index;
            taken[index] = true;
        }
        var r = new T[count];
        for (int i = 0; i < count; i++)
            r[i] = @this[arr[i]];
        return r.ToList();
    }
}