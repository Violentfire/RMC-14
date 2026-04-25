using Content.Server.Explosion.EntitySystems;
using Content.Shared._Sich.Hunter.Caster;
using Robust.Server.GameObjects;

namespace Content.Server._Sich.Hunter.Caster;

public sealed class HunterExplosionSystem : EntitySystem
{
    [Dependency] private readonly ExplosionSystem _explosion = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<ExplodeOnMobHitComponent, TriggerExplosionEvent>(OnTriggerExplosion);
    }

    private void OnTriggerExplosion(EntityUid uid, ExplodeOnMobHitComponent comp, TriggerExplosionEvent args)
    {
        _explosion.QueueExplosion(uid, args.ExplosionId, args.Intensity, 1f, args.Range);
    }
}
