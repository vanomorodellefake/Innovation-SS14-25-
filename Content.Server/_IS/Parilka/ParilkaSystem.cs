using Content.Server.Atmos.EntitySystems;
using Content.Shared.Atmos;
using Content.Shared._IS.Parilka;

namespace Content.Server._IS.Parilka;

public sealed class ParilkaSystem : EntitySystem
{
    [Dependency] private readonly AtmosphereSystem _atmosphereSystem = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<ParilkaComponent, ParilkaSteamEvent>(OnSteam);
    }

    private void OnSteam(EntityUid uid, ParilkaComponent comp, ParilkaSteamEvent args)
    {
        var transform = Transform(uid);

        var environment = _atmosphereSystem.GetContainingMixture((uid, transform), true, true);
        if (environment == null)
            return;

        var merger = new GasMixture(1)
        {
            Temperature = comp.Temperature
        };

        merger.SetMoles(Gas.WaterVapor, args.SteamMoles);
        _atmosphereSystem.Merge(environment, merger);
    }
}
