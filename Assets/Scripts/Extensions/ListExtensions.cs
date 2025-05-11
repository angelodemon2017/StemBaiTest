using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extensions
{
    public static class ListExtensions
    {
        public static T GetRandom<T>(this IEnumerable<T> list)
        {
            return list.ElementAt(Random.Range(0, list.Count()));
        }

        public static void SetIndexes<T>(this List<T> props) where T : IIndexer
        {
            for (int i = 0; i < props.Count(); i++)
            {
                props[i].Index = i;
            }
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int randomIndex = Random.Range(i, list.Count);
                (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
            }
        }
    }
}