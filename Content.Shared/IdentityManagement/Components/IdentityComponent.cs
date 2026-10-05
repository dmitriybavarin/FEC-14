using Robust.Shared.Containers;
using Robust.Shared.Enums;

namespace Content.Shared.IdentityManagement.Components;

/// <summary>
///     Stores the identity entity (whose name is the users 'identity', etc)
///     for a given entity, and marks that it can have an identity at all.
/// </summary>
/// <remarks>
///     This is a <see cref="ContainerSlot"/> and not just a datum entity because we do sort of care that it gets deleted and sent with the user.
/// </remarks>
[RegisterComponent]
public sealed partial class IdentityComponent : Component
{
    [ViewVariables]
    public ContainerSlot IdentityEntitySlot = default!;
}

/// <summary>
///     A data structure representing the 'identity' of an entity as presented to
///     other players.
/// </summary>
public sealed class IdentityRepresentation
{
    public string TrueName;
    public Gender TrueGender;

    public string AgeString;

    public string? PresumedName;
    public string? PresumedJob;

    public IdentityRepresentation(string trueName, Gender trueGender, string ageString, string? presumedName=null, string? presumedJob=null)
    {
        TrueName = trueName;
        TrueGender = trueGender;

        AgeString = ageString;

        PresumedJob = presumedJob;
        PresumedName = presumedName;
    }

    public string ToStringKnown(bool trueName)
    {
        return trueName
            ? TrueName
            : PresumedName ?? ToStringUnknown();
    }

    /// <summary>
    ///     Returns a string representing their identity where it is 'unknown' by a viewer.
    ///     Used for cases where the viewer is not necessarily able to accurately assess
    ///     the identity of the person being viewed.
    /// </summary>
    public string ToStringUnknown()
    {
        var genderString = TrueGender switch
        {
            Gender.Female => Loc.GetString("identity-gender-feminine"),
            Gender.Male => Loc.GetString("identity-gender-masculine"),
            Gender.Epicene or Gender.Neuter or _ => Loc.GetString("identity-gender-person")
        };

        // i.e. 'young assistant man' or 'old cargo technician person' or 'middle-aged captain'
        // FEC14
        var ageId = AgeString == Loc.GetString("identity-age-young") ? "young"
            : AgeString == Loc.GetString("identity-age-old") ? "old"
            : "middle";
        var genderId = TrueGender switch
        {
            Gender.Female => "female",
            Gender.Male => "male",
            _ => "other"
        };

        return PresumedJob is null
            ? Loc.GetString("fec-identity-unknown", ("age", ageId), ("gender", genderId), ("ageString", AgeString), ("genderString", genderString))
            : Loc.GetString("fec-identity-unknown-job", ("age", ageId), ("gender", genderId), ("ageString", AgeString), ("genderString", genderString), ("job", PresumedJob));
        // FEC14
    }
}
