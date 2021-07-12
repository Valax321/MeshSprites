using System;

namespace Valax321.MeshSprites
{
    internal static class ArrayUtility
    {
        public static TResult[] Convert<TSource, TResult>(this TSource[] source, Func<TSource, TResult> operatorFunc)
        {
            var result = new TResult[source.Length];
            for (var i = 0; i < result.Length; i++)
            {
                result[i] = operatorFunc(source[i]);
            }

            return result;
        }
    }
}