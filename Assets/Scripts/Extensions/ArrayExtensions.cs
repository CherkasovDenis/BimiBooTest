using UnityEngine;

namespace BimiBooTest.Extensions
{
    public static class ArrayExtensions
    {
        public static void Shuffle<T>(this T[,] array)
        {
            var lengthRow = array.GetLength(1);

            for (var i = array.Length - 1; i > 0; i--)
            {
                var i0 = i / lengthRow;
                var i1 = i % lengthRow;

                var j = Random.Range(0, i + 1);
                var j0 = j / lengthRow;
                var j1 = j % lengthRow;

                (array[i0, i1], array[j0, j1]) = (array[j0, j1], array[i0, i1]);
            }
        }
    }
}