using System;
using System.Linq;

namespace _Game.Scripts.Extensions
{
    public static class ArrayExtensions
    {
        public static T[] GetUniqueRandomElements<T>(this T[] source, int count)
        {
            count = Math.Min(count, source.Length);
            T[] result = source.ToArray();
            Shuffle(ref result);

            return result.Take(count).ToArray();
        }

        public static void Shuffle<T>(ref T[] inputArray)
        {
            for (int i = 0; i < inputArray.Length; i++)
            {
                T temp = inputArray[i];
                int random = new Random().Next(i, inputArray.Length);
                inputArray[i] = inputArray[random];
                inputArray[random] = temp;
            }
        }
    }
}