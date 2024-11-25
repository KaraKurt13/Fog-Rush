using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MathHelper
{
    public static IEnumerable<int> GetRandomUniqueNumbers(int min, int max, int count)
    {
        List<int> numbers = Enumerable.Range(min, max - min + 1).ToList();

        System.Random random = new System.Random();
        for (int i = 0; i < numbers.Count; i++)
        {
            int randomIndex = random.Next(i, numbers.Count);
            (numbers[i], numbers[randomIndex]) = (numbers[randomIndex], numbers[i]);
        }

        return numbers.Take(count);
    }
}
