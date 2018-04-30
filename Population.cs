using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace PopulationGenerator
{
	public static class Population
	{
		public const byte AvgGenerationAge = 24;
		public const byte MinReproductionAge = 18;
		public const byte MaxFemaleReproductionAge = 50; // menopause

		public static List<Person> People { get; internal set; } = new List<Person>();
		internal static List<Person> BirthQueue = new List<Person>();

		public static void Generate(uint numRootPeople, uint numYears)
		{
			People = new List<Person>((int)(numRootPeople * numYears));
			for (var count = 0; count < numRootPeople; ++count)
			{
				People.Add(new Person());
			}

			var deadPeople = new List<Person>();

			for (var year = 0; year < numYears; ++year)
			{
				foreach (var person in People)
				{
					person.SimulateYear(year);

					if (person.IsDead)
					{
						deadPeople.Add(person);
					}
				}

				foreach (var deadPerson in deadPeople)
				{
					People.Remove(deadPerson);
				}

				deadPeople.Clear();

				foreach (var child in BirthQueue)
				{
					People.Add(child);
				}

				BirthQueue.Clear();
			}
		}

		public static void Print()
		{
			var sb = new StringBuilder();
			foreach (var person in People)
			{
				sb.Append($"Name: {person.Name,32} | Age: {person.Age}\n");
				/*
                sb.Append($"Name: {person.Name}\n");

                if (person.HasParents())
                {
                    sb.Append($"Parents: {person.Mother.Name}, {person.Father.Name}\n");
                }
                else
                {
                    sb.Append("Parents: None\n");
                }

                sb.Append($"Age: {person.Age}\n");
                sb.Append($"Gender: {person.DNA.Gender}\n");
                sb.Append($"Hair Color: {person.DNA.HairColor}\n");
                sb.Append($"Eye Color: {person.DNA.EyeColor}\n");
                sb.Append($"Skin Color: {person.DNA.SkinColor}\n");
                sb.Append($"Height: {person.DNA.Height}\n");
                sb.Append($"Blood Type: {person.DNA.BloodType}\n");
                sb.Append("\n");

                sb.Append("\n\n");
                */
			}

			Console.WriteLine(sb);
		}
	}
}
