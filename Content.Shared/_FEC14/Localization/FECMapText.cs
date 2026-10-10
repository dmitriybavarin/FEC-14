using System.Text;
using Robust.Shared.Localization;

namespace Content.Shared._FEC14.Localization;

public static class FECMapText
{
    public static string Localize(string prefix, string text)
    {
        if (Loc.TryGetString(Key(prefix, text), out var full))
            return full;

        var bracket = text.IndexOf(" (", StringComparison.Ordinal);
        if (bracket > 0 && Loc.TryGetString(Key(prefix, text[..bracket]), out var head))
            return head + text[bracket..];

        return text;
    }

    private static string Key(string prefix, string text)
    {
        var builder = new StringBuilder();
        var dash = false;
        foreach (var ch in text.ToLowerInvariant())
        {
            if (char.IsAscii(ch) && char.IsLetterOrDigit(ch))
            {
                if (dash && builder.Length > 0)
                    builder.Append('-');

                builder.Append(ch);
                dash = false;
            }
            else
            {
                dash = true;
            }
        }

        return prefix + builder;
    }
}
