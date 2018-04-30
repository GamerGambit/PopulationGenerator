using System.Collections.Generic;

namespace PopulationGenerator
{
	public class Person
	{
		public Person Mother { get; private set; } = null;
		public Person Father { get; private set; } = null;
		public Person Partner { get; private set; } = null;

		public DNA DNA { get; private set; }
		public PersonName Name { get; private set; }
		public int Age { get; private set; } = 0;
		public bool IsDead { get; private set; } = false;

		public int YearOfBirth { get; private set; } = 0;
		public int YearOfDeath { get; private set; } = 0;

		public static uint GetRelatedLevel(Person person1, Person person2)
		{
			if (!person1.HasParents() || !person2.HasParents())
				return 0;

			uint count = 0;

			if (person1.Mother == person2.Mother || person1.Father == person2.Father)
				count = 1;

			return count + GetRelatedLevel(person1.Mother, person2.Mother) + GetRelatedLevel(person1.Mother, person2.Father) + GetRelatedLevel(person1.Father, person2.Mother) + GetRelatedLevel(person1.Father, person2.Father);
		}

		public static bool FindSuitableMate(Person mateFor, out Person mate)
		{
			var candidates = new List<Person>();

			foreach (var person in Population.People)
			{
				if (person == mateFor)
					continue;

				if (person.Age < Population.MinReproductionAge)
					continue;

				// TODO: add diversity
				if (person.DNA.Gender == mateFor.DNA.Gender)
					continue;

				// TODO: infidelity?
				if (person.Partner != null)
					continue;

				// TODO: small chance of inbreeding?
				// until GetRelatedLevel is improved and we are able to see in what way 2 people are related, its ruled out completely
				if (GetRelatedLevel(mateFor, person) > 0)
					continue;

				candidates.Add(person);
			}

			if (candidates.Count == 0)
			{
				mate = null;
				return false;
			}

			mate = candidates.Pick();
			return true;
		}

		internal void SimulateYear(int year)
		{
			++Age;

			// Death
			if (Utils.Rnd.NextDouble() <= Age / 1200.0)
			{
				IsDead = true;
				YearOfDeath = year;
				return;
			}

			// Partner (50% chance)
			if (Age >= Population.MinReproductionAge)
			{
				if (Partner == null && Utils.Rnd.Next(101) <= 50)
				{
					if (FindSuitableMate(this, out var mate))
					{
						Partner = mate;
						mate.Partner = this;
					}
				}
				else if (Partner != null && Partner.IsDead == false && Utils.Rnd.Next(101) <= 5) // 5% chance of a breakup
				{
					Partner.Partner = null;
					Partner = null;
				}
			}

			// Reproduction (30% chance)
			if (
				Partner != null && Partner.IsDead == false && Utils.Rnd.Next(101) <= 30 &&
				(
					(DNA.Gender == Gender.Female && Age <= Population.MaxFemaleReproductionAge) ||
					(DNA.Gender == Gender.Male && Partner.Age <= Population.MaxFemaleReproductionAge)
				)
			)
			{
				var mother = (DNA.Gender == Gender.Female) ? this : Partner;
				var father = (DNA.Gender == Gender.Male) ? this : Partner;

				Population.BirthQueue.Add(new Person(year, mother, father));
			}
		}

		public Person()
		{
			DNA = DNA.Generate();
			Name = PersonName.Create(DNA.Gender);
		}

		public Person(int yearOfBirth, Person mother, Person father)
		{
			Mother = mother;
			Father = father;
			DNA = new DNA(mother.DNA, father.DNA);
			Name = new PersonName(DNA.Gender, mother.Name, father.Name);
			YearOfBirth = yearOfBirth;
		}

		public bool HasParents()
		{
			return Mother != null && Father != null;
		}
	}
}