using System;
using System.Linq;
using System.Collections.Generic;

namespace GameUtils
{
    static public class RandomHelper
    {
        private static readonly Random rng = new Random();

        // Returns a random integer n such that min <= n < max
        public static int Range(int minimumInclusive, int maximumExclusive)
        {
            return rng.Next(minimumInclusive, maximumExclusive);
        }

        // Returns a random value v such that 0.0 <= v < 1.0
        public static float Sample()
        {
            return (float)rng.NextDouble();
        }

        public static bool Test(float chanceOfSuccess)
        {
            return Sample() < chanceOfSuccess;
        }
        
        public static T Select<T>(IEnumerable<T> items) where T : IWeighted
        {
            var totalWeight = items.Sum(item => item.Weight);
            var roll = rng.Next(totalWeight);

            foreach (var item in items)
            {
                if (roll < item.Weight)
                {
                    return item;
                }

                roll -= item.Weight;
            }

            return default(T);
        }
    }
    
    public interface IWeighted
    {
        int Weight { get; }
    }
}