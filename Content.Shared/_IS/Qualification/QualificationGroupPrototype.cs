using System.ComponentModel.DataAnnotations;
using Content.Shared.Roles;
using Content.Shared.StatusIcon;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Array;

namespace Content.Shared._IS.Qualification;

[Prototype]
public sealed partial class QualificationGroupPrototype : IPrototype
{
    [ViewVariables]
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField(required: true)]
    public HashSet<ProtoId<JobPrototype>> JobPrototypes = default!;

    [DataField(required: true)]
    public HashSet<ProtoId<QualificationPrototype>> QualificationHashSet = default!;
}
