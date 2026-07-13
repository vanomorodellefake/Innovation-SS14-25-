using System.Linq;
using Content.Server.GameTicking;
using Content.Server.Station.Components;
using Content.Server.Station.Systems;
using Content.Shared._IS.Qualification;
using Content.Shared._IS.Qualification.Components;
using Content.Shared._White.Examine;
using Content.Shared.Players.PlayTimeTracking;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Robust.Shared.Prototypes;

namespace Content.Server._IS.Qualification;

public sealed partial class QualificationSystem : EntitySystem
{
    [Dependency] private StationJobsSystem _stationJobsSystem = default!;
    [Dependency] private IPrototypeManager _prototypeManager = default!;
    [Dependency] private readonly ISharedPlaytimeManager _playtimeManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RulePlayerJobsAssignedEvent>(OnPlayerJobAssigned);
    }

    private void OnPlayerJobAssigned(RulePlayerJobsAssignedEvent args)
    {
        var query = EntityQueryEnumerator<StationJobsComponent>();
        while (query.MoveNext(out var uid, out var sjComp))
        {
            foreach (var session in args.Players)
            {
                if (session.AttachedEntity == null)
                    continue;

                if (!_stationJobsSystem.TryGetPlayerJobs(uid, session.UserId, out var jobs, sjComp))
                    continue;

                var playTimes = _playtimeManager.GetPlayTimes(session);

                foreach (var job in jobs)
                {
                    if (!playTimes.TryGetValue(job, out var time))
                        continue;

                    var qgPrototypes = _prototypeManager.EnumeratePrototypes<QualificationGroupPrototype>();

                    foreach (var qgPrototype in qgPrototypes)
                    {
                        if (qgPrototype.JobPrototypes.Contains(job))
                        {
                            var qPrototypes = qgPrototype.QualificationHashSet;

                            QualificationPrototype? qp = null;

                            foreach (var qPrototype in qPrototypes)
                            {
                                if (!_prototypeManager.TryIndex(qPrototype, out var proto))
                                    continue;

                                if (time < proto.Requirement)
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
        }
    }
}
