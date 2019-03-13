using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonsAndDragons
{
    class Program
    {
        private static readonly Random Random = new Random();

        private static readonly Item[] Items =
        {
            new Item("Fireball", 2, CharacterClass.GetSubset(CharacterClass.Dragonborn)),

            new Item("Boomerang", 1, CharacterClass.GetSubset(CharacterClass.Dwarf)),

            new Item("Sword", 1, CharacterClass.GetSubset(CharacterClass.Elf)),

            new Item("Staff", 1, CharacterClass.GetSubset(CharacterClass.Druid, CharacterClass.Wizard)),

            new Item("Leather", 64, CharacterClass.All),
            new Item("Torch", Item.CountUnlimited, CharacterClass.All),
        };

        private static readonly Stat[] Stats =
        {
            new Stat("Charisma"),
            new Stat("Dexterity"),
            new Stat("Intelligence"),
            new Stat("Strength"),
            new Stat("Wisdom"),
        };

        private static readonly Trait[] Traits =
        {
            new Trait("Fire Immunity", CharacterClass.GetSubset(CharacterClass.Dragonborn)),

            new Trait("Dark Vision", CharacterClass.GetSubset(CharacterClass.Elf)),

            new Trait("Spell Ability", CharacterClass.GetSubset(CharacterClass.Druid, CharacterClass.Wizard)),

            new Trait("Speed", CharacterClass.All),
        };

        static Program()
        {
            Array.Sort(Items);
        }


        static void Main()
        {
            Console.WriteLine("DUNGEONS AND DRAGONS");
            Console.WriteLine();
            
            Character[] players = new Character[Properties.Settings.Default.PlayerCount];

            for (int i = 0; i < players.Length; ++i)
            {
                int playerID = i + 1;

                Console.WriteLine($"Input name of player { playerID } or press ENTER for default.");
                string playerName = Console.ReadLine();

                if (String.IsNullOrEmpty(playerName)) { playerName = $"Player{ playerID }"; --Console.CursorTop; }
                Console.WriteLine();


                // CLASS SELECTION
                Console.WriteLine("Available classes:");
                foreach (CharacterClass characterClass in CharacterClass.All) { Console.WriteLine(characterClass.Name); }
                Console.WriteLine();

                Console.WriteLine($"Input name of class to assign '{ playerName }' or press ENTER for random.");
                CharacterClass playerClass;
                do
                {
                    string className = Console.ReadLine();

                    if (String.IsNullOrEmpty(className))
                    {
                        playerClass = CharacterClass.All.ElementAt(Random.Next(CharacterClass.All.Count));

                        --Console.CursorTop;
                        break;
                    }

                    playerClass = CharacterClass.TryGetFromName(className);
                    if (playerClass == null) { Console.WriteLine("Class does not exist."); continue; }

                    break;
                } while (true);
                Console.WriteLine();


                Character player = new Character(playerName, playerClass);


                // ITEM SELECTION
                foreach (Item item in Items)
                {
                    if (!item.IsCompatibleWith(playerClass)) { continue; }

                    Console.WriteLine($"Input count of '{ item.Name }' to give { playerClass.Name } '{ playerName }' or press ENTER for 0.");
                    Console.WriteLine($"Choose between 0 and { (item.CountMax == -1 ? "however many" : item.CountMax.ToString()) }.");

                    do
                    {
                        string countLine = Console.ReadLine();
                        int count;

                        if (String.IsNullOrEmpty(countLine)) { count = 0; --Console.CursorTop; }
                        else if (!Int32.TryParse(countLine, out count)) { Console.WriteLine("Input is malformed or too large."); continue; }

                        if (!player.Inventory.AddItem(item, count)) { Console.WriteLine("Could not add item."); continue; }

                        break;
                    }
                    while (true);

                    Console.WriteLine();
                }


                // STAT ASSIGNMENT
                foreach (Stat stat in Stats)
                {
                    player.Stats.Add(stat, 1 + Random.Next(100));
                }


                // TRAIT ASSIGNMENT
                foreach (Trait trait in Traits)
                {
                    if (!trait.IsCompatibleWith(playerClass)) { continue; }

                    player.Traits.Add(trait);
                }

                players[i] = player;
            }


            Console.WriteLine($"Input ID of player between 1 and {players.Length} to display information about that player.");
            Console.WriteLine("When done, input EOF (Ctrl+Z) to exit.");
            for (int playerID = 0; ; )
            {
                string line = Console.ReadLine();

                if (line == null) { break; }
                else if (String.IsNullOrEmpty(line))
                {
                    playerID = playerID % players.Length + 1;
                }
                else if (!Int32.TryParse(line, out playerID)) { Console.WriteLine("Input is malformed or too large."); continue; }
                else if (playerID < 1 || playerID > players.Length) { Console.WriteLine("Value is out of bounds."); continue; }
                Console.WriteLine();

                players[playerID - 1].Print(Console.Out);
            }
        }
    }
}
