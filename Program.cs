using System;
using System.Diagnostics;

namespace PopulationGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			Population.Generate(1, 100);
			sw.Stop();

			Population.Print();

			Console.WriteLine($"Generated {Population.People.Count} people (from 10 root people over 100 years) in {sw.ElapsedMilliseconds} ms");
			Console.ReadLine();
		}
	}
}
