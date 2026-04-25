using System;
using Content.Shared._RMC14.Armor.ThermalCloak;
using Content.Shared._RMC14.Chemistry;
using Content.Shared._RMC14.NightVision;
using Content.Shared._RMC14.Stealth;
using Content.Shared._RMC14.Water;
using Content.Shared._Sich.Hunter.Cloak;
using Content.Shared._Sich.Hunter;
using Content.Shared.Actions;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Damage;
using Content.Shared.FixedPoint;
using Content.Shared.Humanoid;
using Content.Shared.Inventory;
using Content.Shared.Inventory.Events;
using Content.Shared.Mobs;
using Content.Shared.Mobs.Systems;
using Content.Shared.Popups;

using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Timing;
using Robust.Shared.Maths;

namespace Content.Shared._Sich.Hunter.Cloak;

public sealed class HunterCloakSystem : EntitySystem
{
    [Dependency] private readonly HunterEnergySystem _hunterEnergy = default!;
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedHumanoidAppearanceSystem _humanoidSystem = default!;
    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly INetManager _net = default!;
    [Dependency] private readonly MobStateSystem _mobState = default!;
    [Dependency] private readonly SharedMapSystem _map = default!;

    [Dependency] private readonly ITileDefinitionManager _tileDefManager = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<HunterCloakComponent, GetItemActionsEvent>(OnGetItemActions);
        SubscribeLocalEvent<HunterCloakComponent, HunterCloakToggleActionEvent>(OnCloakAction);
        SubscribeLocalEvent<HunterCloakComponent, GotUnequippedEvent>(OnUnequipped);

        SubscribeLocalEvent<HunterCloakedComponent, DamageChangedEvent>(OnDamageChanged);
        SubscribeLocalEvent<HunterCloakedComponent, VaporHitEvent>(OnVaporHit);
        SubscribeLocalEvent<HunterCloakedComponent, SolutionTransferredEvent>(OnSolutionTransferred);
        SubscribeLocalEvent<HunterCloakedComponent, MobStateChangedEvent>(OnMobStateChanged);
    }

    private void OnGetItemActions(Entity<HunterCloakComponent> ent, ref GetItemActionsEvent args)
    {
        args.AddAction(ref ent.Comp.Action, ent.Comp.ActionId);
        Dirty(ent);
    }

    private void OnCloakAction(Entity<HunterCloakComponent> ent, ref HunterCloakToggleActionEvent args)
    {
        if (args.Handled)
            return;

        var user = args.Performer;

        // Ensure the cloak is actually equipped by the performer in a valid slot
        var isEquipped = false;
        var slots = _inventory.GetSlotEnumerator(user, SlotFlags.OUTERCLOTHING | SlotFlags.BACK | SlotFlags.GLOVES);
        while (slots.MoveNext(out var slot))
        {
            if (slot.ContainedEntity == ent.Owner)
            {
                isEquipped = true;
                break;
            }
        }

        if (!isEquipped)
            return;

        args.Handled = true;

        if (!ent.Comp.Enabled)
        {
            // Activation check
            if (HasComp<EntityActiveInvisibleComponent>(user))
            {
                _popup.PopupClient(Loc.GetString("hunter-cloak-already-active"), user, user);
                return;
            }

            if (!_hunterEnergy.HasEnergy(user, ent.Comp.MinEnergy))
            {
                _popup.PopupClient(Loc.GetString("hunter-cloak-low-energy"), user, user, PopupType.SmallCaution);
                return;
            }

            // Spend activation energy
            _hunterEnergy.TryUseEnergy(user, ent.Comp.EnergyCost);

            SetInvisibility(ent, user, true, false);
        }
        else
        {
            SetInvisibility(ent, user, false, false);
        }
    }

    private void OnUnequipped(Entity<HunterCloakComponent> ent, ref GotUnequippedEvent args)
    {
        SetInvisibility(ent, args.Equipee, false, false);

        // Actions system handles removing the action automatically since it's provided via GetItemActionsEvent
        ent.Comp.Action = null;
        Dirty(ent.Owner, ent.Comp);
    }


    public void SetInvisibility(Entity<HunterCloakComponent> ent, EntityUid user, bool enabling, bool forced)
    {
        if (enabling)
        {
            if (HasComp<EntityActiveInvisibleComponent>(user))
                return;

            var activeInvisibility = EnsureComp<EntityActiveInvisibleComponent>(user);
            activeInvisibility.Opacity = ent.Comp.Opacity;
            Dirty(user, activeInvisibility);

            EnsureComp<HunterCloakedComponent>(user);
            
            var turnInvisible = EnsureComp<EntityTurnInvisibleComponent>(user);
            turnInvisible.Enabled = true;
            Dirty(user, turnInvisible);

            ent.Comp.Enabled = true;
            Dirty(ent.Owner, ent.Comp);

            if (ent.Comp.Action != null)
                _actions.SetToggled(ent.Comp.Action.Value, true);

            EnsureComp<RMCNightVisionVisibleComponent>(user); // Make sure they are visible to NV
            
            SpawnCloakEffects(user, ent.Comp.CloakEffect);

            var popupOthers = Loc.GetString("hunter-cloak-activate-others", ("user", user));
            _popup.PopupPredicted(Loc.GetString("hunter-cloak-activate-self"), popupOthers, user, user, PopupType.Medium);

            if (_net.IsServer)
                _audio.PlayPvs(ent.Comp.CloakSound, user);

            return;
        }

        // Deactivating
        ent.Comp.Enabled = false;
        Dirty(ent.Owner, ent.Comp);

        if (ent.Comp.Action != null)
            _actions.SetToggled(ent.Comp.Action.Value, false);

        if (TryComp<EntityActiveInvisibleComponent>(user, out var invisible))
        {
            if (forced)
            {
                var forcedPopupOthers = Loc.GetString("hunter-cloak-forced-deactivate-others", ("user", user));
                _popup.PopupPredicted(Loc.GetString("hunter-cloak-forced-deactivate-self"), forcedPopupOthers, user, user, PopupType.Medium);
            }
            else
            {
                var popupOthers = Loc.GetString("hunter-cloak-deactivate-others", ("user", user));
                _popup.PopupPredicted(Loc.GetString("hunter-cloak-deactivate-self"), popupOthers, user, user, PopupType.Medium);
            }

            SpawnCloakEffects(user, ent.Comp.UncloakEffect);
            
            RemCompDeferred<HunterCloakedComponent>(user);
            RemCompDeferred<EntityActiveInvisibleComponent>(user);

            if (TryComp<EntityTurnInvisibleComponent>(user, out var turnInvisible))
            {
                turnInvisible.Enabled = false;
                turnInvisible.UncloakTime = _timing.CurTime;
                Dirty(user, turnInvisible);
                RemCompDeferred<EntityTurnInvisibleComponent>(user);
            }

            if (_net.IsServer)
                _audio.PlayPvs(ent.Comp.UncloakSound, user);
        }
    }

    private void OnDamageChanged(Entity<HunterCloakedComponent> ent, ref DamageChangedEvent args)
    {
        if (!args.DamageIncreased || args.DamageDelta == null)
            return;

        var cloaks = GetWornCloaks(ent.Owner);
        if (cloaks.Count == 0)
            return;

        var totalDamage = args.DamageDelta.GetTotal();
        foreach (var cloak in cloaks)
        {
            if (totalDamage > cloak.Comp.DamageBreakThreshold)
            {
                SetInvisibility(cloak, ent.Owner, false, true);
            }
            else
            {
                // Flicker effect
                ent.Comp.FlickerUntil = _timing.CurTime + TimeSpan.FromSeconds(cloak.Comp.FlickerDuration);
                Dirty(ent.Owner, ent.Comp);
            }
        }
    }

    private void OnVaporHit(Entity<HunterCloakedComponent> ent, ref VaporHitEvent args)
    {
        foreach (var cloak in GetWornCloaks(ent.Owner))
        {
            SetInvisibility(cloak, ent.Owner, false, true);
        }
    }

    private void OnSolutionTransferred(Entity<HunterCloakedComponent> ent, ref SolutionTransferredEvent args)
    {
        if (args.To != ent.Owner)
            return;

        foreach (var cloak in GetWornCloaks(ent.Owner))
        {
            SetInvisibility(cloak, ent.Owner, false, true);
        }
    }

    private void OnMobStateChanged(Entity<HunterCloakedComponent> ent, ref MobStateChangedEvent args)
    {
        if (args.NewMobState != MobState.Dead && args.NewMobState != MobState.Critical)
            return;

        foreach (var cloak in GetWornCloaks(ent.Owner))
        {
            SetInvisibility(cloak, ent.Owner, false, true);
        }
    }

    public List<Entity<HunterCloakComponent>> GetWornCloaks(EntityUid player)
    {
        var result = new List<Entity<HunterCloakComponent>>();
        var slots = _inventory.GetSlotEnumerator(player, SlotFlags.OUTERCLOTHING | SlotFlags.BACK | SlotFlags.GLOVES);
        while (slots.MoveNext(out var slot))
        {
            if (TryComp<HunterCloakComponent>(slot.ContainedEntity, out var comp))
                result.Add((slot.ContainedEntity.Value, comp));
        }

        return result;
    }

    public void SpawnCloakEffects(EntityUid user, EntProtoId cloakProtoId)
    {
        if (_net.IsClient)
            return;

        var xform = Transform(user);
        Spawn(cloakProtoId, xform.Coordinates);
    }


    public override void Update(float frameTime)
    {
        // Cloak effects & Drain
        var query = EntityQueryEnumerator<HunterCloakedComponent, TransformComponent>();
        while (query.MoveNext(out var uid, out var cloaked, out var xform))
        {
            var cloaks = GetWornCloaks(uid);
            if (cloaks.Count == 0)
            {
                RemCompDeferred<HunterCloakedComponent>(uid);
                continue;
            }

            // Dynamic Energy Drain
            if (HasComp<HunterEnergyComponent>(uid))
            {
                var drain = 0f;
                foreach (var cloak in cloaks)
                {
                    drain += cloak.Comp.PassiveEnergyCost;
                }

                if (!_hunterEnergy.TryUseEnergy(uid, drain * frameTime))
                {
                    foreach (var cloak in cloaks)
                    {
                        SetInvisibility(cloak, uid, false, true);
                    }
                }
            }

            // Flicker Visuals
            if (TryComp<EntityActiveInvisibleComponent>(uid, out var invisible))
            {
                var isFlickering = _timing.CurTime < cloaked.FlickerUntil;
                var targetOpacity = 0.1f; // Default

                if (isFlickering)
                {
                    foreach (var cloak in cloaks)
                    {
                        targetOpacity = Math.Max(targetOpacity, cloak.Comp.FlickerOpacity);
                    }
                }
                else
                {
                    foreach (var cloak in cloaks)
                    {
                        targetOpacity = cloak.Comp.Opacity;
                    }
                }

                if (!MathHelper.CloseTo(invisible.Opacity, targetOpacity))
                {
                    invisible.Opacity = targetOpacity;
                    Dirty(uid, invisible);
                }
            }

            // Water check
            if (xform.GridUid is { } gridUid && TryComp<MapGridComponent>(gridUid, out var mapGrid))
            {
                var tile = _map.GetTileRef(gridUid, mapGrid, xform.Coordinates);
                var tileDef = _tileDefManager[tile.Tile.TypeId];
                var isWater = tileDef.ID.Contains("water", StringComparison.OrdinalIgnoreCase);

                if (!isWater)
                {
                    var anchored = _map.GetAnchoredEntities(gridUid, mapGrid, tile.GridIndices);
                    foreach (var anchoredId in anchored)
                    {
                        if (HasComp<RMCWaterComponent>(anchoredId))
                        {
                            isWater = true;
                            break;
                        }
                    }
                }

                if (isWater)
                {
                    foreach (var cloak in cloaks)
                    {
                        SetInvisibility(cloak, uid, false, true);
                    }
                }
            }
        }
    }
}
