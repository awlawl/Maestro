using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicData
{
    public static class IEnumerableExtentions
    {
        //from http://stackoverflow.com/questions/5807128/an-extension-method-on-ienumerable-needed-for-shuffling
       public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, Random rng)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (rng == null) throw new ArgumentNullException("rng");

            return source.ShuffleIterator(rng);
        }

       public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source)
       {
           if (source == null) throw new ArgumentNullException("source");

           return source.ShuffleIterator(new Random());
       }

        private static IEnumerable<T> ShuffleIterator<T>(
            this IEnumerable<T> source, Random rng)
        {
            var buffer = source.ToList();

            for (int i = 0; i < buffer.Count; i++)
            {
                int j = rng.Next(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }
}
