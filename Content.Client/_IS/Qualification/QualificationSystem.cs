using Content.Client.Overlays;
using Content.Shared._IS.Qualification;
using Content.Shared._IS.Qualification.Components;
using Content.Shared.Access.Components;
using Content.Shared.Access.Systems;
using Content.Shared.Examine;
using Content.Shared.Overlays;
using Content.Shared.PDA;
using Content.Shared.StatusIcon.Components;
using Robust.Shared.Prototypes;

namespace Content.Client._IS.Qualification;

public sealed partial class QualificationSystem : EntitySystem
{
    [Dependency] private readonly IPrototypeManager _prototype = default!;
    [Dependency] private readonly AccessReaderSystem _accessReader = default!;

    public override void Initialize()
    {
        base.Initialize();

        //SubscribeLocalEvent<QualificationComponent, GetStatusIconsEvent>(OnGetStatusIconsEvent);
        SubscribeLocalEvent<QualificationComponent, ExaminedEvent>(OnExamine);
    }

    /*

    private void OnGetStatusIconsEvent(Entity<QualificationComponent> entity, ref GetStatusIconsEvent args)
    {
        if (!IsActive)
            return;

        if (!TryComp<QualificationComponent>(entity, out var qComp))
            return;

        ProtoId<QualificationPrototype>? iconId = null;

        if (_accessReader.FindAccessItemsInventory(entity, out var items))
        {
            foreach (var item in items)
            {
                // ID Card
                if (TryComp<IdCardComponent>(item, out var id))
                {
                    iconId = qComp.QualificationIcon;
                    break;
                }

                // PDA
                if (TryComp<PdaComponent>(item, out var pda)
                    && pda.ContainedId != null
                    && TryComp(pda.ContainedId, out id))
                {
                    iconId = qComp.QualificationIcon;
                    break;
                }
            }
        }

        if (iconId == null)
            return;

        if (_prototype.TryIndex(iconId, out var iconPrototype))
            args.StatusIcons.Add(iconPrototype);
        else
            Log.Error($"Invalid job icon prototype: {iconPrototype}");
    } */

    private void OnExamine(Entity<QualificationComponent> entity, ref ExaminedEvent args)
    {
        if (!CheckIDCard(entity)
            || !args.IsInDetailsRange)
            return;

        ProtoId<QualificationPrototype>? iconId = entity.Comp.QualificationIcon;
        _prototype.TryIndex(iconId, out var iconPrototype);

        if (iconPrototype == null)
            return;

        var locale = iconPrototype.QualificationTitle;

        args.PushText(Loc.GetString(locale),
            5);
    }

    private bool CheckIDCard(EntityUid entity)
    {
        if (_accessReader.FindAccessItemsInventory(entity, out var items))
        {
            foreach (var item in items)
            {
                // ID Card
                if (HasComp<IdCardComponent>(item))
                {
                    return true;
                }

                // PDA
                if (TryComp<PdaComponent>(item, out var pda)
                    && pda.ContainedId != null
                    && HasComp<IdCardComponent>(pda.ContainedId))
                {
                    return true;
                }
            }
        }

        return false;
    }
}
