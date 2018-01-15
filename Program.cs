using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PopulationGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Population.Generate(150, 4);
            sw.Stop();
            Console.WriteLine("Generated in {0} ms", sw.ElapsedMilliseconds);
            Console.ReadLine();
        }
    }
}
