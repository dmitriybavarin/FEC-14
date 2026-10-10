using System.Collections.Frozen;

namespace Content.Shared._FEC14.Radio;

public static class FECRadioKeyAliases
{
    public static readonly FrozenDictionary<char, char[]> Aliases = new Dictionary<char, char[]>
    {
        ['a'] = ['а'],
        ['b'] = ['б'],
        ['c'] = ['с', 'ц'],
        ['d'] = ['д'],
        ['e'] = ['е'],
        ['f'] = ['ф'],
        ['g'] = ['г'],
        ['h'] = ['х'],
        ['j'] = ['й'],
        ['k'] = ['к'],
        ['l'] = ['л'],
        ['m'] = ['м'],
        ['n'] = ['н'],
        ['o'] = ['о'],
        ['p'] = ['п'],
        ['r'] = ['р'],
        ['t'] = ['т'],
        ['u'] = ['у'],
        ['v'] = ['в'],
        ['w'] = ['ш'],
        ['z'] = ['з'],
    }.ToFrozenDictionary();
}
