using Content.Shared._Sich.Hunter;
using Content.Shared._Sich.Hunter.Weapon;
using Content.Shared.Actions;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction.Events;
using Content.Shared.Popups;
using Content.Shared.Throwing;
using Content.Shared.Hands.Components;
using Content.Shared.Inventory;
using Robust.Shared.Containers;
using Robust.Shared.Map;

namespace Content.Server._Sich.Hunter.Weapon;

public sealed class HunterRecallWeaponSystem : EntitySystem
{
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly HunterEnergySystem _hunterEnergy = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HunterRecallWeaponComponent, ThrownEvent>(OnThrown);
        SubscribeLocalEvent<HunterRecallWeaponComponent, DroppedEvent>(OnDropped);
        SubscribeLocalEvent<HunterRecallActionEvent>(OnRecallAction);
        SubscribeLocalEvent<HunterRecallWeaponComponent, ComponentShutdown>(OnShutdown);
    }

    private void OnThrown(EntityUid uid, HunterRecallWeaponComponent component, ThrownEvent args)
    {
        if (args.User == null)
            return;

        BindWeapon(uid, component, args.User.Value);
    }

    private void OnDropped(EntityUid uid, HunterRecallWeaponComponent component, DroppedEvent args)
    {
        BindWeapon(uid, component, args.User);
    }

    private void BindWeapon(EntityUid uid, HunterRecallWeaponComponent component, EntityUid user)
    {
        // Clear previous bound weapons for this user
        var query = EntityQueryEnumerator<HunterRecallWeaponComponent>();
        while (query.MoveNext(out var otherUid, out var otherComp))
        {
            if (otherComp.BoundOwner == user && otherUid != uid)
            {
                BreakLink(otherUid, otherComp);
            }
        }

        component.BoundOwner = user;
        
        // Spawn chain
        if (component.ChainEntity == null)
        {
            var chain = Spawn("HunterRecallChain", _transform.GetMapCoordinates(uid));
            if (TryComp<HunterRecallChainComponent>(chain, out var chainComp))
            {
                chainComp.Source = user;
                chainComp.Target = uid;
                component.ChainEntity = chain;
                Dirty(chain, chainComp);
            }
        }
        
        Dirty(uid, component);
    }

    private void OnRecallAction(HunterRecallActionEvent args)
    {
        var performer = args.Performer;

        // Find the weapon bound to this performer
        var query = EntityQueryEnumerator<HunterRecallWeaponComponent>();
        while (query.MoveNext(out var uid, out var component))
        {
            if (component.BoundOwner != performer)
                continue;

            if (args.Handled)
                continue;

            // Check if already in hands
            if (_container.IsEntityInContainer(uid))
            {
                if (_container.TryGetContainingContainer(uid, out var container))
                {
                    if (container.Owner == performer)
                    {
                        // Already has it?
                        continue;
                    }
                }
            }

            // Validate distance
            var userPos = _transform.GetWorldPosition(performer);
            var weaponPos = _transform.GetWorldPosition(uid);
            if ((userPos - weaponPos).Length() > component.MaxRecallRange)
            {
                BreakLink(uid, component, "chain-broken-range");
                continue;
            }

            // Check if in someone else's inventory
            if (_container.TryGetContainingContainer(uid, out var outerContainer))
            {
                var owner = outerContainer.Owner;
                if (owner != performer && (HasComp<InventoryComponent>(owner) || HasComp<HandsComponent>(owner)))
                {
                    BreakLink(uid, component, "chain-broken-container");
                    continue;
                }
            }

            // Energy check
            if (!_hunterEnergy.HasEnergy(performer, component.RecallCost))
            {
                _popup.PopupEntity("Вам не вистачає енергії для повернення зброї!", performer, performer);
                continue;
            }

            // Recall!
            if (_hands.TryForcePickupAnyHand(performer, uid))
            {
                _hunterEnergy.TryUseEnergy(performer, component.RecallCost);
                BreakLink(uid, component); // Success, remove chain
                args.Handled = true;
            }
            else
            {
                // Drop at feet if hands full
                _transform.SetWorldPosition(uid, userPos);
                BreakLink(uid, component);
                args.Handled = true;
            }
        }
    }

    private void BreakLink(EntityUid uid, HunterRecallWeaponComponent component, string? message = null)
    {
        if (message != null && component.BoundOwner != null)
        {
            _popup.PopupEntity(Loc.GetString(message), component.BoundOwner.Value, component.BoundOwner.Value);
        }

        if (component.ChainEntity != null)
        {
            QueueDel(component.ChainEntity.Value);
            component.ChainEntity = null;
        }

        component.BoundOwner = null;
        Dirty(uid, component);
    }

    private void OnShutdown(EntityUid uid, HunterRecallWeaponComponent component, ComponentShutdown args)
    {
        if (component.ChainEntity != null)
            QueueDel(component.ChainEntity.Value);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        // Periodic checks
        var query = EntityQueryEnumerator<HunterRecallWeaponComponent>();
        while (query.MoveNext(out var uid, out var component))
        {
            if (component.BoundOwner == null)
                continue;

            var owner = component.BoundOwner.Value;

            // Distance check
            var userPos = _transform.GetWorldPosition(owner);
            var weaponPos = _transform.GetWorldPosition(uid);
            if ((userPos - weaponPos).Length() > component.MaxRecallRange)
            {
                BreakLink(uid, component, "chain-broken-range");
                continue;
            }

            // Container check
            if (_container.TryGetContainingContainer(uid, out var container))
            {
                var containerOwner = container.Owner;
                if (containerOwner != owner && (HasComp<InventoryComponent>(containerOwner) || HasComp<HandsComponent>(containerOwner)))
                {
                    BreakLink(uid, component, "chain-broken-container");
                    continue;
                }
            }
        }
    }
}
