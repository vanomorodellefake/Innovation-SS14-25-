namespace Content.Shared._IS.Parilka;

[RegisterComponent]
public sealed partial class ParilkaComponent : Component
{
    [DataField("fuel")]
    public int Fuel = 0;

    [DataField("maxFuel")]
    public int MaxFuel = 100;

    [DataField("active")]
    public bool Active = false;

    [DataField("temperature")]
    public float Temperature = 0f;

    [DataField("burnTime")]
    public float BurnTime = 0f;

    [DataField("heatPerSecond")]
    public float HeatPerSecond = 15f;

    [DataField("usePerSecond")]
    public int UsePerSecond = 1;

    [DataField("fuelUseInterval")]
    public float FuelUseInterval = 1f;

    [DataField("steamTimer")]
    public float SteamTimer = 0f;

    [DataField("steamInterval")]
    public float SteamInterval = 7f;
    [DataField("steamVisualTimer")]
    public float SteamVisualTimer = 0f;
    [DataField("steamVisualDuration")]
    public float SteamVisualDuration = 0.8f;
}
