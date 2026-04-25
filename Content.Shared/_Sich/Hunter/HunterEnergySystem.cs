using Content.Shared.Alert;
using Content.Shared.FixedPoint;
using Content.Shared.Rounding;
using Robust.Shared.Timing;
using Content.Shared._Sich.Hunter.Caster;

namespace Content.Shared._Sich.Hunter;

public sealed class HunterEnergySystem : EntitySystem
{
    [Dependency] private readonly AlertsSystem _alerts = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<HunterEnergyComponent, MapInitEvent>(OnMapInit);
        SubscribeLocalEvent<HunterEnergyComponent, ComponentRemove>(OnRemove);
    }

    private void OnMapInit(Entity<HunterEnergyComponent> ent, ref MapInitEvent args)
    {
        UpdateAlert(ent);
    }

    private void OnRemove(Entity<HunterEnergyComponent> ent, ref ComponentRemove args)
    {
        _alerts.ClearAlert(ent, ent.Comp.Alert);
    }

    public override void Update(float frameTime)
    {
        var query = EntityQueryEnumerator<HunterEnergyComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var energy, out var xform))
        {
            if (energy.Energy >= energy.MaxEnergy)
                continue;

            var regen = energy.EnergyRegen;
            if (xform.GridUid is { } gridUid && HasComp<HunterShipComponent>(gridUid))
            {
                regen = energy.EnergyRegenShip;
            }

            RegenEnergy((uid, energy), regen * frameTime);
        }
    }

    public void UpdateAlert(Entity<HunterEnergyComponent> hunter)
    {
        if (hunter.Comp.MaxEnergy != 0)
        {
            var level = MathF.Max(0f, hunter.Comp.Energy.Float());
            var max = _alerts.GetMaxSeverity(hunter.Comp.Alert);
            var severity = ContentHelpers.RoundToLevels(level, hunter.Comp.MaxEnergy.Float(), max + 1);
            
            // Detailed message: Energy / Max
            string? energyMessage = $"Заряд: {(int) hunter.Comp.Energy} / {(int) hunter.Comp.MaxEnergy}";
            
            _alerts.ShowAlert(hunter, hunter.Comp.Alert, (short)severity, dynamicMessage: energyMessage);
        }
    }

    public bool HasEnergy(Entity<HunterEnergyComponent?> hunter, FixedPoint2 amount)
    {
        if (!Resolve(hunter, ref hunter.Comp, false))
            return false;

        return hunter.Comp.Energy >= amount;
    }

    public bool TryUseEnergy(Entity<HunterEnergyComponent?> hunter, FixedPoint2 amount)
    {
        if (!Resolve(hunter, ref hunter.Comp, false))
            return false;

        if (hunter.Comp.Energy < amount)
            return false;

        hunter.Comp.Energy -= amount;
        Dirty(hunter);
        UpdateAlert((hunter, hunter.Comp));
        return true;
    }

    public void RegenEnergy(Entity<HunterEnergyComponent?> hunter, FixedPoint2 amount)
    {
        if (!Resolve(hunter, ref hunter.Comp, false))
            return;

        var old = hunter.Comp.Energy;
        hunter.Comp.Energy = FixedPoint2.Min(hunter.Comp.Energy + amount, hunter.Comp.MaxEnergy);

        if (old != hunter.Comp.Energy)
        {
            Dirty(hunter);
            UpdateAlert((hunter, hunter.Comp));
        }
    }
    
    public void SetEnergy(Entity<HunterEnergyComponent?> hunter, FixedPoint2 amount)
    {
        if (!Resolve(hunter, ref hunter.Comp, false))
            return;

        hunter.Comp.Energy = FixedPoint2.Min(amount, hunter.Comp.MaxEnergy);
        Dirty(hunter);
        UpdateAlert((hunter, hunter.Comp));
    }
}
