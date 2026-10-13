using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Content.Shared._RMC14.IdentityManagement;
using Robust.Shared.Enums;
using Robust.Shared.GameObjects.Components.Localization;

namespace Content.Shared._FEC14.Localization;

public sealed class FECRussianCases
{
    [Dependency] private readonly IEntityManager _entity = default!;
    [Dependency] private readonly ILocalizationManager _loc = default!;

    private static readonly string[] Cases = { "gen", "dat", "acc", "ins", "prep" };

    public void Initialize(CultureInfo culture)
    {
        _loc.AddFunction(culture, "CASE", FuncCase);
    }

    private ILocValue FuncCase(LocArgs args)
    {
        if (args.Args.Count == 0)
            return new LocValueString(string.Empty);

        var value = args.Args[0];
        var grammaticalCase = args.Args.Count > 1 ? args.Args[1].Format(new LocContext()).Trim().ToLowerInvariant() : "nom";
        if (!Cases.Contains(grammaticalCase))
            return new LocValueString(value.Format(new LocContext()));

        if (value is IdentityEntity identity)
            return new LocValueString(DeclineIdentity(identity, grammaticalCase));

        if (value.Value is EntityUid uid)
            return new LocValueString(DeclineEntity(uid, _entity.GetComponentOrNull<MetaDataComponent>(uid)?.EntityName ?? string.Empty, grammaticalCase));

        var text = value.Format(new LocContext());
        return new LocValueString(LooksLikeName(text) ? DeclineName(text, GuessGender(text), grammaticalCase) : text);
    }

    private static bool LooksLikeName(string text)
    {
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        return words.Length > 0 &&
               words.Any(w => w.Any(IsCyrillic)) &&
               words.All(w => w.EndsWith('.') || char.IsUpper(w[0]));
    }

    private static Gender GuessGender(string text)
    {
        var words = text.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Where(w => !w.EndsWith('.'))
            .Select(w => w.ToLowerInvariant())
            .ToArray();

        if (words.Length == 0)
            return Gender.Male;

        var last = words[^1];
        if (EndsWithAny(last, "ова", "ева", "ёва", "ина", "ына", "ская", "цкая", "ая"))
            return Gender.Female;

        if (words.Length > 1 && EndsWithAny(words[0], "а", "я") && EndsWithAny(last, "а", "я"))
            return Gender.Female;

        return Gender.Male;
    }

    private string DeclineIdentity(IdentityEntity identity, string grammaticalCase)
    {
        var realName = _entity.GetComponentOrNull<MetaDataComponent>(identity.Entity)?.EntityName;
        if (!string.Equals(realName, identity.Name, StringComparison.Ordinal))
            return identity.Name;

        return DeclineEntity(identity.Entity, identity.Name, grammaticalCase);
    }

    private string DeclineEntity(EntityUid uid, string name, string grammaticalCase)
    {
        if (TryGetPrototypeCase(uid, name, grammaticalCase, out var form))
            return form;

        if (!IsProper(uid))
            return name;

        return DeclineName(name, GetGender(uid), grammaticalCase);
    }

    private bool TryGetPrototypeCase(EntityUid uid, string name, string grammaticalCase, [NotNullWhen(true)] out string? form)
    {
        form = null;
        if (_entity.TryGetComponent(uid, out GrammarComponent? grammar) &&
            grammar.Attributes.TryGetValue(grammaticalCase, out form))
        {
            return true;
        }

        if (_entity.GetComponentOrNull<MetaDataComponent>(uid)?.EntityPrototype is not { } prototype)
            return false;

        var data = _loc.GetEntityData(prototype.ID);
        if (!string.Equals(data.Name, name, StringComparison.Ordinal))
            return false;

        return data.Attributes.TryGetValue(grammaticalCase, out form);
    }

    private bool IsProper(EntityUid uid)
    {
        if (_entity.TryGetComponent(uid, out GrammarComponent? grammar) && grammar.ProperNoun is { } proper)
            return proper;

        if (_entity.GetComponentOrNull<MetaDataComponent>(uid)?.EntityPrototype is { } prototype &&
            _loc.GetEntityData(prototype.ID).Attributes.TryGetValue("proper", out var attr))
        {
            return attr == "true";
        }

        return false;
    }

    private Gender GetGender(EntityUid uid)
    {
        if (_entity.TryGetComponent(uid, out GrammarComponent? grammar) && grammar.Gender is { } gender)
            return gender;

        return Gender.Male;
    }

    public static string DeclineName(string name, Gender gender, string grammaticalCase)
    {
        var words = name.Split(' ');
        for (var i = 0; i < words.Length; i++)
        {
            words[i] = DeclineWord(words[i], gender, grammaticalCase);
        }

        return string.Join(' ', words);
    }

    private static string DeclineWord(string word, Gender gender, string c)
    {
        if (word.Length < 2 || word.EndsWith('.') || !word.All(ch => ch == '-' || IsCyrillic(ch)) || !char.IsUpper(word[0]))
            return word;

        if (word.Contains('-'))
            return string.Join('-', word.Split('-').Select(part => DeclineWord(part, gender, c)));

        var lower = word.ToLowerInvariant();
        var female = gender == Gender.Female;

        if (female)
        {
            if (EndsWithAny(lower, "ова", "ева", "ёва", "ина", "ына"))
                return Swap(word, 1, c switch { "acc" => "у", _ => "ой" });

            if (EndsWithAny(lower, "ская", "цкая", "ая"))
                return Swap(word, 2, c switch { "acc" => "ую", _ => "ой" });

            if (lower.EndsWith("ия"))
                return Swap(word, 1, c switch { "gen" or "dat" or "prep" => "и", "acc" => "ю", _ => "ей" });

            if (lower.EndsWith('я'))
                return Swap(word, 1, c switch { "gen" => "и", "dat" or "prep" => "е", "acc" => "ю", _ => "ей" });

            if (lower.EndsWith('а'))
                return Swap(word, 1, c switch { "gen" => SoftGen(lower) ? "и" : "ы", "dat" or "prep" => "е", "acc" => "у", _ => Hush(lower) ? "ей" : "ой" });

            return word;
        }

        if (EndsWithAny(lower, "ский", "цкий", "ой", "ый"))
            return Swap(word, 2, c switch { "gen" or "acc" => "ого", "dat" => "ому", "ins" => EndsWithAny(lower, "кий") ? "им" : "ым", _ => "ом" });

        if (lower.EndsWith("ий"))
            return Swap(word, 2, c switch { "gen" or "acc" => "ия", "dat" => "ию", "ins" => "ием", _ => "ии" });

        if (lower.EndsWith('й'))
            return Swap(word, 1, c switch { "gen" or "acc" => "я", "dat" => "ю", "ins" => "ем", _ => "е" });

        if (lower.EndsWith('ь'))
            return Swap(word, 1, c switch { "gen" or "acc" => "я", "dat" => "ю", "ins" => "ем", _ => "е" });

        if (lower.EndsWith('я'))
            return Swap(word, 1, c switch { "gen" => "и", "dat" or "prep" => "е", "acc" => "ю", _ => "ей" });

        if (lower.EndsWith('а'))
            return Swap(word, 1, c switch { "gen" => SoftGen(lower) ? "и" : "ы", "dat" or "prep" => "е", "acc" => "у", _ => Hush(lower) ? "ей" : "ой" });

        if (EndsWithAny(lower, "о", "е", "и", "у", "ю", "ы", "э"))
            return word;

        if (EndsWithAny(lower, "ов", "ев", "ёв", "ин", "ын"))
            return word + c switch { "gen" or "acc" => "а", "dat" => "у", "ins" => "ым", _ => "е" };

        return word + c switch { "gen" or "acc" => "а", "dat" => "у", "ins" => Hush(lower + "о") ? "ем" : "ом", _ => "е" };
    }

    private static bool IsCyrillic(char ch)
    {
        return ch is >= 'а' and <= 'я' or >= 'А' and <= 'Я' or 'ё' or 'Ё';
    }

    private static bool EndsWithAny(string word, params string[] endings)
    {
        return endings.Any(word.EndsWith);
    }

    private static bool SoftGen(string lower)
    {
        return lower.Length > 1 && "гкхжчшщ".Contains(lower[^2]);
    }

    private static bool Hush(string lower)
    {
        return lower.Length > 1 && "жчшщц".Contains(lower[^2]);
    }

    private static string Swap(string word, int cut, string ending)
    {
        return word[..^cut] + ending;
    }
}
