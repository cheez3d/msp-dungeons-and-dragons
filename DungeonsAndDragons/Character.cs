using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DungeonsAndDragons
{
    class Character
    {
        public Character(string name, CharacterClass characterClass)
        {
            Name = name;
            Class = characterClass;
            Inventory = new Inventory<Item>();
            Stats = new SortedList<Stat, int>();
            Traits = new List<Trait>();
        }


        public string Name { get; }

        public CharacterClass Class { get; }

        public Inventory<Item> Inventory { get; }

        public SortedList<Stat, int> Stats { get; }

        public List<Trait> Traits { get; }


        public void Print(TextWriter textWriter)
        {
            Console.WriteLine($"Information for {Class.Name} '{Name}'");
            Console.WriteLine();

            Console.WriteLine("Inventory:");
            Inventory.Print(textWriter);
            Console.WriteLine();

            Console.WriteLine("Stats:");
            foreach (KeyValuePair<Stat, int> entry in Stats)
            {
                Console.WriteLine($"{entry.Key.Name} = {entry.Value}/100");
            }
            Console.WriteLine();

            Console.WriteLine("Traits:");
            foreach (Trait trait in Traits)
            {
                Console.WriteLine(trait.Name);
            }
            Console.WriteLine();
        }
    }

    partial class CharacterClass
    {
        private static readonly Dictionary<string, CharacterClass> StringToCharacterClass = new Dictionary<string, CharacterClass>();
        static CharacterClass()
        {
            foreach (CharacterClass characterClass in All)
            {
                StringToCharacterClass.Add(characterClass.Name, characterClass);
            }
        }

        public static CharacterClass TryGetFromName(string name) => StringToCharacterClass.TryGetValue(name, out CharacterClass characterClass) ? characterClass : null;


        private static readonly Dictionary<string, IReadOnlyCollection<CharacterClass>> SubsetsMemoized = new Dictionary<string, IReadOnlyCollection<CharacterClass>>();

        public static IReadOnlyCollection<CharacterClass> GetSubset(params CharacterClass[] characterClasses)
        {
            string key = String.Join(":", characterClasses.Select(x => x.Name));

            if (SubsetsMemoized.TryGetValue(key, out IReadOnlyCollection<CharacterClass> subset)) { return subset; }

            SubsetsMemoized.Add(key, subset = new HashSet<CharacterClass>(characterClasses));

            return subset;
        }


        private CharacterClass(string name)
        {
            Name = name;
        }


        public string Name { get; }
    }

    partial class CharacterClass
    {
        public static readonly CharacterClass Dragonborn = new CharacterClass("Dragonborn");
        public static readonly CharacterClass Druid = new CharacterClass("Druid");
        public static readonly CharacterClass Dwarf = new CharacterClass("Dwarf");
        public static readonly CharacterClass Elf = new CharacterClass("Elf");
        public static readonly CharacterClass Wizard = new CharacterClass("Wizard");

        public static readonly IReadOnlyCollection<CharacterClass> All = GetSubset(Dragonborn, Druid, Dwarf, Elf, Wizard);
    }
}
