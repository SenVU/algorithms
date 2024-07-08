using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public static class Array2D<T>
{
    /// <summary>
    /// gets a random location in a 2D array using (Environment.TickCount) as the seed
    /// </summary>
    public static T GetRandomForm2DArray(T[,] array, bool nonNull = true)
    {
        return GetRandomForm2DArray(array, Environment.TickCount, nonNull);
    }

    /// <summary>
    /// gets a random location in a 2D array using the given seed
    /// </summary>
    public static T GetRandomForm2DArray(T[,] array, int seed, bool nonNull = true)
    {
        Random random = new Random(seed);
        List<T> arrayList = null;

        arrayList = new List<T>();
        foreach (T obj in array)
        {
            if (!nonNull || obj != null)
                arrayList.Add(obj);
        }

        if (arrayList.Count == 0) return default(T);
        return arrayList[random.Next(0, arrayList.Count())];
    }
}
