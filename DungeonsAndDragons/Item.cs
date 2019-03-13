using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonsAndDragons
{
    class Item : IClassRestricted, IComparable<Item>
    {
        public static readonly int CountUnlimited = -1;


        public Item(string name, int countMax, IReadOnlyCollection<CharacterClass> classesAllowed)
        {
            Name = name;
            CountMax = countMax;
            ClassesAllowed = classesAllowed;
        }


        public IReadOnlyCollection<CharacterClass> ClassesAllowed { get; }

        public string Name { get; }

        public int CountMax { get; }


        public int CompareTo(Item other) => Name.CompareTo(other.Name);

        public bool IsCompatibleWith(CharacterClass characterClass)
        {
            return ClassesAllowed.Contains(characterClass);
        }
    }
}
