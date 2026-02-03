using Content.Shared.Stacks;
using Content.Shared.Interaction;
using Content.Shared.Tag;
using Robust.Shared.Prototypes;
using Content.Shared.Item.ItemToggle.Components;
using Content.Shared.IgnitionSource;
using Content.Shared.Temperature;
using System;





namespace Content.Shared._IS.Parilka;

public sealed class ParilkaSystem : EntitySystem
{
            [Dependency] private readonly TagSystem _tag = default!;
            [Dependency] private readonly SharedStackSystem _stack = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<ParilkaComponent, InteractUsingEvent>(OnInteractUsing);
    }

    private bool IsHot(EntityUid used)
    {
        var ev = new IsHotEvent();
        RaiseLocalEvent(used, ev);
        return ev.IsHot;
    }


    private void OnInteractUsing(EntityUid uid, ParilkaComponent component, InteractUsingEvent args)
    {
        var used = args.Used;
        if (_tag.HasTag(used, "FuelForFire"))
        {
            if (component.Fuel >= component.MaxFuel)
                return;

            if (!EntityManager.TryGetComponent<StackComponent>(used, out var stack))
                return;
            if (!_stack.Use(used, 1, stack!))
                return;
            component.Fuel = Math.Min(component.Fuel + 1, component.MaxFuel);
            args.Handled = true;
        }




        if (!IsHot(used))
            return;

        if (component.Fuel <= 0)
            return;
        component.Active = true;
        args.Handled = true;
    }
}
