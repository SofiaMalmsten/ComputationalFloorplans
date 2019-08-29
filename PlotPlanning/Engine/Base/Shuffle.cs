using System;
using System.Collections.Generic;
using System.Linq;

namespace PlotPlanning.Engine.Base
{
    public partial class Modify
    {
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random random)
        {

            T[] elements = source.ToArray();
            for (int i = elements.Length - 1; i >= 0; i--)
            {
                // Swap element "i" with a random earlier element it (or itself)
                // ... except we don't really need to swap it fully, as we can
                // return it immediately, and afterwards it's irrelevant.
                int swapIndex = random.Next(i + 1);
                yield return elements[swapIndex];
                elements[swapIndex] = elements[i];
            }
        }
    }

    //====================================================================//
}
