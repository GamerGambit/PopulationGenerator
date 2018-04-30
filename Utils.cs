using System;
using System.Collections.Generic;

namespace PopulationGenerator
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

		public static T Pick<T>(this List<T> list)
		{
			return list[Rnd.Next(list.Count)];
		}

		public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
		{
			if (value.CompareTo(min) < 0)
				return min;

			if (value.CompareTo(max) > 0)
				return max;

			return value;
		}
	}
}
