using Content.Shared.Atmos;
using Content.Shared._IS.Parilka;
using Content.Server.Atmos.EntitySystems;
using Robust.Shared.Map.Components;

namespace Content.Server._IS.Parilka;

public sealed class ParilkaSystem : EntitySystem
{
    [Dependency] private readonly AtmosphereSystem _atmos = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<ParilkaComponent, ParilkaSteamEvent>(OnSteam);
    }

    private void OnSteam(EntityUid uid, ParilkaComponent comp, ParilkaSteamEvent args)
    {
        var xform = Transform(uid);

        var cord = _atmos.GetTileMixture((uid, xform), true);
        if (cord is not { })
            return;

        cord.AdjustMoles(Gas.WaterVapor, args.SteamMoles);

    }


}
