namespace PopulationGenerator
{
    public class Person
    {
        public DNA DNA;
        public PersonName Name;
        
        public Person(PersonName name, DNA dna)
        {
            Name = name;
            DNA = dna;
        }

        public Person(PersonName name, Person mother, Person father)
        {
            Name = name;
            DNA = new DNA(mother.DNA, father.DNA);
        }
    }
}
