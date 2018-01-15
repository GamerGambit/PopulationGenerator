using System;

namespace InfiniteDetectiveAncestryTree
{
    public static class Utils
    {
        public static Random Rnd = new Random();

        public static T Pick<T>(params T[] args)
        {
            return args[Rnd.Next(args.Length)];
        }

        public static T Pick<T>() where T : struct
        {
            var values = Enum.GetValues(typeof(T));
            return (T)values.GetValue(Rnd.Next(values.Length));
        }
    }
}
