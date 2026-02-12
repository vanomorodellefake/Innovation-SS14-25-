using Robust.Shared.GameStates;

namespace Content.Shared._IS.Parilka;

public sealed class ParilkaSteamEvent : EntityEventArgs
{
    public float SteamMoles;

    public ParilkaSteamEvent(float steamMoles)
    {
        SteamMoles = steamMoles;
    }

} //no comments
