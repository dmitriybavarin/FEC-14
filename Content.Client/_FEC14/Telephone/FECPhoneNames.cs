using Content.Shared._FEC14.Localization;

namespace Content.Client._FEC14.Telephone;

public static class FECPhoneNames
{
    public static string Localize(string text)
    {
        return FECMapText.Localize("fec-phone-", text);
    }
}
