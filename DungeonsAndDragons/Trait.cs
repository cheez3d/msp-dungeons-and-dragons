using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonsAndDragons
{
    class Trait : IClassRestricted, IComparable<Trait>
    {
        public Trait(string name, IReadOnlyCollection<CharacterClass> classesAllowed)
        {
            Name = name;
            ClassesAllowed = classesAllowed;
        }


        public IReadOnlyCollection<CharacterClass> ClassesAllowed { get; }

        public string Name { get; }


        public int CompareTo(Trait other) => Name.CompareTo(other.Name);

        public bool IsCompatibleWith(CharacterClass characterClass)
        {
            return ClassesAllowed.Contains(characterClass);
        }
    }
}
