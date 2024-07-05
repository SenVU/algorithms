using GXPEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public static class Array2D<T>
{
    public static bool FindIn2DArray(T[,] array, T value)
    {
        foreach (T val in array)
        {
            if (val != null && val.Equals(value)) return true;
        }
        return false;
    }

    //public static void Clear2DArray(T[,] array, int width, int height)
    //{
    //    for (int i = 0; i < array.Length; i++)
    //        array[] = default(T);
    //}


    public static T GetRandomForm2DArray(T[,] array, bool nonNull = true)
    {
        return GetRandomForm2DArray(array, Environment.TickCount, nonNull);
    }

    public static T GetRandomForm2DArray(T[,] array, int seed, bool nonNull = true)
    {
        Random random = new Random(seed);
        List <T> arrayList = new List<T> { };
        foreach (T val in array)
        {
            if (!nonNull || val != null)
                arrayList.Add(val);
        }
        if (arrayList.Count == 0) return default(T);
        return arrayList[random.Next(0, arrayList.Count())];
    }

    /*public static T GetValueFrom2DArray(T[,] array, int x, int y)
    {
        if (array.Length>x && array[x].Length>y)
            return array[x][y];
        return default(T);
    }*/
}
