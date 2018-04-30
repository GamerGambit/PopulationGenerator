using System;
using System.Diagnostics;

namespace PopulationGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			var generateArgs = (150u, 100u);

			Stopwatch sw = new Stopwatch();
			sw.Start();
			Population.Generate(generateArgs.Item1, generateArgs.Item2);
			sw.Stop();

			Population.Print();

			Console.WriteLine($"Generated {Population.People.Count} people (from {generateArgs.Item1} root people over {generateArgs.Item2} years) in {sw.ElapsedMilliseconds} ms");
			Console.ReadLine();
		}
	}
}
