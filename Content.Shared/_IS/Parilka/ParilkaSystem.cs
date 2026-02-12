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

    public override void Update(float frameTime)
    {
        var query = EntityQueryEnumerator<ParilkaComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (!comp.Active)
                continue;

            comp.BurnTime += frameTime;
            if (comp.BurnTime < 1f)
                continue;

            comp.BurnTime -= 1f;

            if (comp.Fuel <= 0)
            {
                comp.Active = false;
                continue;
            }

            comp.Fuel = Math.Max(0, comp.Fuel - (int)comp.UsePerSecond);
            comp.Temperature += comp.HeatPerSecond;

        }
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
