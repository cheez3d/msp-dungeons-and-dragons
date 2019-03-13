using System;
using System.Collections.Generic;

namespace DungeonsAndDragons
{
    class Stat : IComparable<Stat>
    {
        public Stat(string name)
        {
            Name = name;
        }


        public IReadOnlyCollection<CharacterClass> ClassesAllowed { get; }

        public string Name { get; }


        public int CompareTo(Stat other) => Name.CompareTo(other.Name);
    }
}
