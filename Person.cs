using System;

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

        internal Person SimulateYear(int year)
        {
            ++Age;

            // Death
            if (Utils.Rnd.NextDouble() <= Age / 1200.0)
            {
                IsDead = true;
                YearOfDeath = year;
                return null;
            }

            // Partner (50% chance)
            if (Age >= Population.MinReproductionAge)
            {
                if (Partner == null && Utils.Rnd.Next(101) <= 50)
                {
                    var foundPartner = Population.People.Find(p => p.DNA.Gender != DNA.Gender && p.Age >= Population.MinReproductionAge);

                    if (foundPartner != null)
                    {
                        Partner = foundPartner;
                        foundPartner.Partner = this;
                    }
                }
                else if (Partner != null && Partner.IsDead == false && Utils.Rnd.Next(101) <= 5) // 5% chance of a breakup
                {
                    Partner.Partner = null;
                    Partner = null;
                }
            }

            // Reproduction (30% chance)
            if (Partner != null && Partner.IsDead == false && Utils.Rnd.Next(101) <= 30 && ((DNA.Gender == Gender.Female && Age <= Population.MaxFemaleReproductionAge) || (DNA.Gender == Gender.Male && Partner.Age <= Population.MaxFemaleReproductionAge)))
            {
                var mother = (DNA.Gender == Gender.Female) ? this : Partner;
                var father = (DNA.Gender == Gender.Male) ? this : Partner;

                return new Person(year, mother, father);
            }

            return null;
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
