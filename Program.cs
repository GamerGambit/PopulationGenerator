using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PopulationGenerator
{
    class Program
    {
        static DNA GenerateDNA()
        {
            return new DNA(Utils.Pick<Gender>(), Utils.Pick<HairColor>(), Utils.Pick<EyeColor>(), Utils.Pick<SkinColor>(), Utils.Pick<Height>(), new BloodType(Utils.Pick<BloodGroup>(), Utils.Pick<BloodRHFactor>()));
        }

        static void generatePopulation(int numRootPeople)
        {
            const int numGenerations = 4;
            var generations = new List<List<Person>>();

            // generate root generation
            {
                generations.Add(new List<Person>());
                var rootGen = generations[0];

                for (int rootCount = 0; rootCount < numRootPeople; ++rootCount)
                {
                    var dna = GenerateDNA();
                    rootGen.Add(new Person(PersonName.Create(dna.Gender), dna));
                }
            }

            for (int genCount = 1; genCount < numGenerations; ++genCount)
            {
                generations.Add(new List<Person>());
                var curGen = generations[genCount];

                var prevGen = generations[genCount - 1];
                var tappedDNAIDs = new List<UInt16>();

                int prevGenPopulationBias = 0;
                prevGen.ForEach(p => prevGenPopulationBias += p.DNA.Gender == Gender.Female ? 1 : -1);
                var maxCouples = (prevGen.Count - Math.Abs(prevGenPopulationBias)) / 2;
                for (int coupleCount = 0; coupleCount < maxCouples; ++coupleCount)
                {
                    Person mother = null;
                    Person father = null;

                    foreach (var person in prevGen)
                    {
                        if (person.DNA.Gender == Gender.Female && mother == null && tappedDNAIDs.Contains(person.DNA.UniqueID) == false)
                        {
                            mother = person;
                            tappedDNAIDs.Add(person.DNA.UniqueID);
                        }

                        if (person.DNA.Gender == Gender.Male && father == null && tappedDNAIDs.Contains(person.DNA.UniqueID) == false)
                        {
                            father = person;
                            tappedDNAIDs.Add(person.DNA.UniqueID);
                        }

                        if (father != null && mother != null)
                            break;
                    }

                    Debug.Assert(mother != null && father != null);

                    var numChildren = Utils.Rnd.Next(0, 6); // WRONG! @todo replace with normal distribution (3, 1)
                    for (int childCount = 0; childCount < numChildren; ++childCount)
                    {
                        var dna = new DNA(mother.DNA, father.DNA);
                        curGen.Add(new Person(new PersonName(dna.Gender, mother.Name, father.Name), dna));
                    }
                }
            }

            /*
            for (int genCount = 0; genCount < generations.Count; ++genCount)
            {
                Console.WriteLine("Generation {0}", genCount);
                Console.WriteLine("========================");

                var prevGen = new List<Person>();
                if (genCount > 0)
                {
                    prevGen = generations[genCount - 1];
                }

                foreach (var person in generations[genCount])
                {
                    Console.WriteLine("Name: {0}", person.Name);

                    if (person.DNA.HasParents())
                    {
                        Console.WriteLine("Parents: {0}, {1}", prevGen.Find(p => p.DNA.UniqueID == person.DNA.MotherUniqueID).Name, prevGen.Find(p => p.DNA.UniqueID == person.DNA.FatherUniqueID).Name);
                    }
                    else
                    {
                        Console.WriteLine("Parents: None");
                    }

                    Console.WriteLine("Gender: {0}", Enum.GetValues(typeof(Gender)).GetValue((int)person.DNA.Gender));
                    Console.WriteLine("Hair Color: {0}", Enum.GetValues(typeof(HairColor)).GetValue((int)person.DNA.HairColor));
                    Console.WriteLine("Eye Color: {0}", Enum.GetValues(typeof(EyeColor)).GetValue((int)person.DNA.EyeColor));
                    Console.WriteLine("Skin Color: {0}", Enum.GetValues(typeof(SkinColor)).GetValue((int)person.DNA.SkinColor));
                    Console.WriteLine("Height: {0}", Enum.GetValues(typeof(Height)).GetValue((int)person.DNA.Height));
                    Console.WriteLine("Blood Type: {0}", person.DNA.BloodType);
                    Console.WriteLine("\n\n");
                }

                Console.WriteLine("\n\n");
            }
            */
        }

        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            generatePopulation(150);
            sw.Stop();
            Console.WriteLine("Generated in {0} ms", sw.ElapsedMilliseconds);
            Console.ReadLine();
        }
    }
}
