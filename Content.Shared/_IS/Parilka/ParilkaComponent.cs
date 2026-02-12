using Robust.Shared.Prototypes;

namespace Content.Shared._IS.Parilka;



/// <summary>
/// Да хуй знает
/// </summary>

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
    [DataField("HeatPerSecond")]
    public float HeatPerSecond = 15f;
    [DataField("UsePerSecond")]
    public int UsePerSecond = 1;

    [DataField("water")]
    public int Water = 0;
    [DataField("maxWater")]
    public int MaxWater = 50;
    [DataField("steamTimer")]
    public float SteamTimer = 0f;


    // [DataField(readOnly: true, required: true)]
    // public ProtoId<ParilkaPrototype> PariklaProto;

}
