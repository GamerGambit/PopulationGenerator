using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

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
                        curGen.Add(new Person(mother, father));
                    }
                }
            }
        }

        public static void Print()
        {
            var sb = new StringBuilder();

            for (int genCount = 0; genCount < Generations.Count; ++genCount)
            {
                sb.Append($"Generation {genCount}\n========================\n");

                var prevGen = new List<Person>();
                if (genCount > 0)
                {
                    prevGen = Generations[genCount - 1];
                }

                foreach (var person in Generations[genCount])
                {
                    sb.Append($"Name: {person.Name}\n");

                    if (person.HasParents())
                    {
                        sb.Append($"Parents: {person.Mother.Name}, {person.Father.Name}\n");
                    }
                    else
                    {
                        sb.Append("Parents: None\n");
                    }

                    sb.AppendFormat("Gender: {0}\n", Enum.GetValues(typeof(Gender)).GetValue((int)person.DNA.Gender));
                    sb.AppendFormat("Hair Color: {0}\n", Enum.GetValues(typeof(HairColor)).GetValue((int)person.DNA.HairColor));
                    sb.AppendFormat("Eye Color: {0}\n", Enum.GetValues(typeof(EyeColor)).GetValue((int)person.DNA.EyeColor));
                    sb.AppendFormat("Skin Color: {0}\n", Enum.GetValues(typeof(SkinColor)).GetValue((int)person.DNA.SkinColor));
                    sb.AppendFormat("Height: {0}\n", Enum.GetValues(typeof(Height)).GetValue((int)person.DNA.Height));
                    sb.AppendFormat("Blood Type: {0}\n", person.DNA.BloodType);
                    sb.Append("\n");
                }

                sb.Append("\n\n");
            }

            Console.WriteLine(sb);
        }
    }
}
