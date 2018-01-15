namespace PopulationGenerator
{
    public class Person
    {
        public Person Mother = null;
        public Person Father = null;

        public DNA DNA;
        public PersonName Name;
        
        public Person(PersonName name, DNA dna)
        {
            DNA = dna;
            Name = name;
        }

        public Person(Person mother, Person father)
        {
            Mother = mother;
            Father = father;
            DNA = new DNA(mother.DNA, father.DNA);
            Name = new PersonName(DNA.Gender, mother.Name, father.Name);
        }

        public bool HasParents()
        {
            return Mother != null && Father != null;
        }
    }
}
