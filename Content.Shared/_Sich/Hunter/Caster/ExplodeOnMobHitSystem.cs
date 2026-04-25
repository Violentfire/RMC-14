using Content.Shared.Explosion;
using Content.Shared.Projectiles;
using Content.Shared.Mobs.Components;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Sich.Hunter.Caster;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ExplodeOnMobHitComponent : Component
{
    [DataField, AutoNetworkedField]
    public ProtoId<ExplosionPrototype> ExplosionId = "Default";

    [DataField, AutoNetworkedField]
    public float Range = 1.5f;

    [DataField, AutoNetworkedField]
    public int Intensity = 10;
}

public sealed class ExplodeOnMobHitSystem : EntitySystem
{
    [Dependency] private readonly EntityLookupSystem _lookup = default!;
    // Note: Explosion switching to server-side logic if needed, but for now we define the shared hook
    
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ExplodeOnMobHitComponent, ProjectileHitEvent>(OnProjectileHit);
    }

    private void OnProjectileHit(EntityUid uid, ExplodeOnMobHitComponent comp, ref ProjectileHitEvent args)
    {
        if (!HasComp<MobStateComponent>(args.Target))
            return;

        // In RMC, explosions are usually handled on the server.
        // We'll raise a local event that the server-half of this system will catch.
        RaiseLocalEvent(uid, new TriggerExplosionEvent(comp.ExplosionId, comp.Range, comp.Intensity));
    }
}

[Serializable, NetSerializable]
public sealed class TriggerExplosionEvent(ProtoId<ExplosionPrototype> explosionId, float range, int intensity) : EntityEventArgs
{
    public readonly ProtoId<ExplosionPrototype> ExplosionId = explosionId;
    public readonly float Range = range;
    public readonly int Intensity = intensity;
}
