using System;
using Robust.Shared.Serialization;

namespace Content.Shared._IS.Parilka;

[Serializable, NetSerializable]
public enum ParilkaVisuals
{
    IsRunning,
    ShowSteam
}

[Serializable, NetSerializable]
public enum ParilkaVisualLayers : byte
{
    Base,
    Fire,
    Steam
}
