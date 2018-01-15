using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PopulationGenerator
{
    public static class Population
    {
        public static List<List<Person>> Generations { get; private set; } = new List<List<Person>>();

        public static void Generate(uint numRootPeople, uint numGenerations)
        {
            Generations = new List<List<Person>>();

            // generate root generation
            {
                Generations.Add(new List<Person>());
                var rootGen = Generations[0];

                for (int rootCount = 0; rootCount < numRootPeople; ++rootCount)
                {
                    var dna = DNA.Generate();
                    rootGen.Add(new Person(PersonName.Create(dna.Gender), dna));
                }
            }

            for (int genCount = 1; genCount < numGenerations; ++genCount)
            {
                Generations.Add(new List<Person>());
                var curGen = Generations[genCount];

                var prevGen = Generations[genCount - 1];
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
        }

        public static void Print()
        {
            for (int genCount = 0; genCount < Generations.Count; ++genCount)
            {
                Console.WriteLine("Generation {0}", genCount);
                Console.WriteLine("========================");

                var prevGen = new List<Person>();
                if (genCount > 0)
                {
                    prevGen = Generations[genCount - 1];
                }

                foreach (var person in Generations[genCount])
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
                    Console.WriteLine("\n");
                }

                Console.WriteLine("\n\n");
            }
        }
    }
}
