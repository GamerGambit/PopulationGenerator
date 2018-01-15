using System;
using System.Diagnostics;

namespace PopulationGenerator
{
    public enum BloodGroup
    {
        O,
        A,
        B,
        AB
    }

    public enum BloodRHFactor
    {
        Positive,
        Negative
    }

    public sealed class BloodType
    {
        public BloodGroup Group { get; private set; }
        public BloodRHFactor RHFactor { get; private set; }

        public BloodType(BloodGroup group, BloodRHFactor rhFactor)
        {
            Group = group;
            RHFactor = rhFactor;
        }

        public override string ToString()
        {
            return Group.ToString() + (RHFactor == BloodRHFactor.Negative ? '-' : '+');
        }

        public static bool AreGroups(BloodType mother, BloodType father, BloodGroup group1, BloodGroup group2)
        {
            return (mother.Group == group1 && father.Group == group2) || (mother.Group == group2 && father.Group == group1);
        }

        public static BloodType Derive(BloodType mother, BloodType father)
        {
            var rnd = new Random();

            BloodRHFactor rhFactor;

            if (mother.RHFactor == BloodRHFactor.Negative && father.RHFactor == BloodRHFactor.Negative)
            {
                rhFactor = BloodRHFactor.Negative;
            }
            else
            {
                rhFactor = Utils.Pick<BloodRHFactor>();
            }

            BloodGroup group;

            if (mother.Group == BloodGroup.O && father.Group == BloodGroup.O) // O and O
            {
                group = BloodGroup.O;
            }
            else if (AreGroups(mother, father, BloodGroup.O, BloodGroup.A)) // O and A
            {
                group = Utils.Pick(BloodGroup.O, BloodGroup.A);
            }
            else if (AreGroups(mother, father, BloodGroup.O, BloodGroup.B)) // O and B
            {
                group = Utils.Pick(BloodGroup.O, BloodGroup.B);
            }
            else if (AreGroups(mother, father, BloodGroup.O, BloodGroup.AB)) // O and AB
            {
                group = Utils.Pick(BloodGroup.A, BloodGroup.B);
            }
            else if (mother.Group == BloodGroup.A && father.Group == BloodGroup.A) // A and A
            {
                group = Utils.Pick(BloodGroup.O, BloodGroup.A);
            }
            else if (AreGroups(mother, father, BloodGroup.A, BloodGroup.B)) // A and B
            {
                if (rnd.Next(100) <= 50.0)
                {
                    group = Utils.Pick(BloodGroup.O, BloodGroup.A);
                }
                else
                {
                    group = (BloodGroup)rnd.Next((int)BloodGroup.B, (int)BloodGroup.AB + 1);
                }
            }
            else if (AreGroups(mother, father, BloodGroup.A, BloodGroup.AB)) // A and AB
            {
                if (rnd.Next(100) <= 33.0)
                {
                    group = BloodGroup.A;
                }
                else if (rnd.Next(100) <= 33.0)
                {
                    group = BloodGroup.B;
                }
                else
                {
                    group = BloodGroup.AB;
                }
            }
            else if (mother.Group == BloodGroup.B && father.Group == BloodGroup.B) // B and B
            {
                group = Utils.Pick(BloodGroup.O, BloodGroup.B);
            }
            else if (AreGroups(mother, father, BloodGroup.B, BloodGroup.AB)) // B and AB
            {
                if (rnd.Next(100) <= 33.0)
                {
                    group = BloodGroup.B;
                }
                else if (rnd.Next(100) <= 33.0)
                {
                    group = BloodGroup.A;
                }
                else
                {
                    group = BloodGroup.AB;
                }
            }
            else if (mother.Group == BloodGroup.AB && father.Group == BloodGroup.AB) // AB and AB
            {
                if (rnd.Next(100) <= 33.0)
                {
                    group = BloodGroup.A;
                }
                else if (rnd.Next(100) <= 33.0)
                {
                    group = BloodGroup.B;
                }
                else
                {
                    group = BloodGroup.AB;
                }
            }
            else
            {
                group = BloodGroup.O;
                Debug.Assert(false);
            }

            return new BloodType(group, rhFactor);
        }
    }
}
