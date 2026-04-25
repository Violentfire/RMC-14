using Content.Shared.CombatMode;
using Content.Shared._Sich.Hunter;
using Content.Shared._Sich.Hunter.Caster;
using Content.Shared.DoAfter;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared._RMC14.Power;
using Robust.Shared.Timing;
using Robust.Shared.Map.Components;
using Content.Shared.Destructible;
using Content.Shared.Alert;
using Robust.Shared.Prototypes;

namespace Content.Server._Sich.Hunter.Caster;

public sealed class HunterPowerSystem : EntitySystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly SharedDoAfterSystem _doAfter = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedMapSystem _map = default!;
    [Dependency] private readonly HunterEnergySystem _hunterEnergy = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<InteractHandEvent>(OnInteractHand);
        SubscribeLocalEvent<RMCApcComponent, HunterSiphonDoAfterEvent>(OnSiphonDoAfter);
        SubscribeLocalEvent<HunterShipComponent, MapInitEvent>(OnShipMapInit);
    }

    private void OnShipMapInit(EntityUid uid, HunterShipComponent comp, MapInitEvent args)
    {
        if (TryComp<MapLightComponent>(uid, out var light))
        {
            light.AmbientLightColor = new Color(0.1f, 0.08f, 0.05f); // Dim dark-brown/sepia tone
            Dirty(uid, light);
        }
    }

    private void OnInteractHand(InteractHandEvent args)
    {
        if (args.Handled)
            return;

        var uid = args.Target;
        if (!TryComp<RMCApcComponent>(uid, out var apc))
            return;

        if (!TryComp<CombatModeComponent>(args.User, out var combat) || combat.CanDisarm == false)
            return;

        if (!HasComp<HunterEnergyComponent>(args.User))
            return;

        if (TryComp<SichApcSiphonedComponent>(uid, out var siphoned) && 
            _timing.CurTime < siphoned.LastSiphonTime + siphoned.SiphonCooldown)
        {
            _popup.PopupEntity("Енергія в цьому ЗКЖ вичерпана або він надто пошкоджений.", uid, args.User);
            return;
        }

        var ev = new HunterSiphonDoAfterEvent();
        var doAfterArgs = new DoAfterArgs(EntityManager, args.User, 2.0f, ev, uid, uid)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            RequireCanInteract = true
        };

        if (_doAfter.TryStartDoAfter(doAfterArgs))
            args.Handled = true;
    }

    private void OnSiphonDoAfter(EntityUid uid, RMCApcComponent apc, HunterSiphonDoAfterEvent args)
    {
        if (args.Handled || args.Cancelled)
            return;

        var siphoned = EnsureComp<SichApcSiphonedComponent>(uid);
        siphoned.SiphonCount++;
        siphoned.LastSiphonTime = _timing.CurTime;

        // Restore energy to the hunter
        if (TryComp<HunterEnergyComponent>(args.User, out var energy))
        {
            _hunterEnergy.RegenEnergy((args.User, energy), 200); // Siphon gives 200 energy
        }

        if (siphoned.SiphonCount >= 3)
        {
            var breakEv = new BreakageEventArgs();
            RaiseLocalEvent(uid, breakEv);
            _popup.PopupEntity("ЗКЖ остаточно зламано після викачування енергії!", uid, args.User);
        }
        else
        {
            _popup.PopupEntity($"Викачування енергії... ({siphoned.SiphonCount}/3)", uid, args.User);
        }
        
        args.Handled = true;
    }
}

