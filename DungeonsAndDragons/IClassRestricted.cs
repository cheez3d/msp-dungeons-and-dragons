using System.Collections.Generic;

namespace DungeonsAndDragons
{
    interface IClassRestricted
    {
        IReadOnlyCollection<CharacterClass> ClassesAllowed { get; }
        
        bool IsCompatibleWith(CharacterClass characterClass);
    }
}
