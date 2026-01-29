using Content.Shared.Stacks;
using Content.Shared.Interaction;
using Content.Shared.Tag;
using Robust.Shared.Prototypes;
using System;





namespace Content.Shared._IS.Parilka;

public sealed class ParilkaSystem : EntitySystem
{
            [Dependency] private readonly TagSystem _tag = default!;
            [Dependency] private readonly StackSystem _stack = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<ParilkaComponent, InteractUsingEvent>(OnInteractUsing);
    }



    private void OnInteractUsing(EntityUid uid, ParilkaComponent component, InteractUsingEvent args)
    {
        var used = args.Used;
        if (!_tag.HasTag(used, "FuelForFire"))
            return;


        if (component.Fuel >= component.MaxFuel)
                return;

        if (_stack.TryChangeStackCount(used, -1)){
                component.Fuel = Math.Min(component.Fuel + 1, component.MaxFuel);
                args.Handled = true;
        }
    }
}
