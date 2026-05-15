// Licensed under IS14's EULA, see EULA.txt for more information.


using Content.Shared.Buckle
;using Content.Shared.Buckle.Components
;using Robust.Shared.Containers;

namespace Content.Shared._IS14.BunkBed;

public sealed class BunkBedMattressStrapSystem : EntitySystem
{
    [Dependency] private readonly SharedBuckleSystem _buckle = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<BunkBedMattressStrapLinkComponent, ComponentStartup>(OnStartup);
        SubscribeLocalEvent<BunkBedMattressStrapLinkComponent, EntInsertedIntoContainerMessage>(OnInserted);
        SubscribeLocalEvent<BunkBedMattressStrapLinkComponent, EntRemovedFromContainerMessage>(OnRemoved);
    }

    private void OnStartup(EntityUid uid, BunkBedMattressStrapLinkComponent comp, ComponentStartup args)
    {
        UpdateStrap(uid, comp);
    }

    private void OnInserted(EntityUid uid, BunkBedMattressStrapLinkComponent comp, EntInsertedIntoContainerMessage args)
    {
        if (args.Container.ID != comp.SlotContainerId)
            return;

        UpdateStrap(uid, comp);
    }

    private void OnRemoved(EntityUid uid, BunkBedMattressStrapLinkComponent comp, EntRemovedFromContainerMessage args)
    {
        if (args.Container.ID != comp.SlotContainerId)
            return;

        UpdateStrap(uid, comp);
    }

    private void UpdateStrap(EntityUid uid, BunkBedMattressStrapLinkComponent comp)
    {
        if (!TryComp<StrapComponent>(uid, out var strap))
            return;

        if (!_container.TryGetContainer(uid, comp.SlotContainerId, out var container))
            return;

        var occupied = container.ContainedEntities.Count != 0;
        _buckle.StrapSetEnabled(uid, occupied, strap);
    }
}
