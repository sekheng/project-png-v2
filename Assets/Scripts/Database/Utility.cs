using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utility {

    // 2-dimensional
    public static float DistSqr(float x1, float x2, float y1, float y2)
    {
        float distSqr = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1);
        return distSqr;
    }

    // 3-dimensional
    public static float DistSqr(float x1, float x2, float y1, float y2, float z1, float z2)
    {
        float distSqr = (x2 - x1) * (x2 - x1) + (y2 - y1) * (y2 - y1) + (z2 - z1) * (z2 - z1);
        return distSqr;
    }

    /// <summary>
    /// Random int with exclusion generator
    /// </summary>
    /// <param name="min">Minimum number.</param>
    /// <param name="max">Maximum number.</param>
    /// <param name="exclude">Excluded number.</param>
    /// <returns>Returns a random int from min (inclusive) to max (exclusive) without the excluded</returns>
    public static int RandomIntExcept(int min, int max, int exclude)
    {
        if (min > max)
            return min;

        int maxChoices = max - min;
        int[] validChoices = new int[maxChoices - 1];
        for (int i = 0; i < maxChoices - 1; i++)
        {
            if (min + i < exclude)
            {
                validChoices[i] = min + i;
            }
            else
            {
                validChoices[i] = min + i + 1;
            }
        }

        return validChoices[Random.Range(0, validChoices.Length)];
    }

}
