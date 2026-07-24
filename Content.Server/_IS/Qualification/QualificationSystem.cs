using Content.Shared._IS.Qualification;
using Content.Shared._IS.Qualification.Components;
using Content.Shared.GameTicking;
using Content.Shared.Players.PlayTimeTracking;
using Content.Shared.Roles;
using Robust.Shared.Prototypes;

namespace Content.Server._IS.Qualification;

public sealed partial class QualificationSystem : EntitySystem
{
    [Dependency] private IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly ISharedPlaytimeManager _playtimeManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<PlayerSpawnCompleteEvent>(OnPlayerJobAssigned);
    }

    private void OnPlayerJobAssigned(PlayerSpawnCompleteEvent args)
    {
        if (args.JobId == null
            || args.Player.AttachedEntity == null)
            return;

        var jobId = args.JobId;

        if (!_prototypeManager.TryIndex<JobPrototype>(jobId, out var jobProto)
            || jobProto == null)
            return;

        var jobTimeTracker = jobProto.PlayTimeTracker;
        var session = args.Player;
        var playTimes = _playtimeManager.GetPlayTimes(session);

        if (!playTimes.TryGetValue(jobTimeTracker, out var time))
            return;

        var qgPrototypes = _prototypeManager.EnumeratePrototypes<QualificationGroupPrototype>();

        foreach (var qgPrototype in qgPrototypes)
        {
            if (qgPrototype.JobPrototypes.Contains(jobId))
            {
                var qPrototypes = qgPrototype.QualificationHashSet;

                QualificationPrototype? qp = null;

                foreach (var qPrototype in qPrototypes)
                {
                    if (!_prototypeManager.TryIndex(qPrototype, out var proto))
                        continue;

                    var hourTimeRquirement = time.TotalHours;

                    if (hourTimeRquirement >= proto.Requirement)
                        qp = proto;
                }

                if (qp == null)
                    continue;

                var qComp = EnsureComp<QualificationComponent>(session.AttachedEntity.Value);
                qComp.QualificationIcon = qp;
            }
        }
    }
}
