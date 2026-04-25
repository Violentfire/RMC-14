using Content.Shared.Actions;
using Content.Shared._RMC14.Hunter.Components;
using Content.Shared._RMC14.NightVision;
using Content.Shared._RMC14.Scoping;
using Content.Shared._RMC14.Visor;
using Content.Shared.Inventory;
using Content.Shared.Inventory.Events;
using Content.Shared.Popups;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Containers;
using Robust.Shared.Timing;

namespace Content.Shared._RMC14.Hunter.Systems;

public sealed class RMCHunterNightVisionSystem : EntitySystem
{
    [Dependency] private readonly SharedAudioSystem _audio = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly SharedPopupSystem _popup = default!;
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly VisorSystem _visor = default!;

    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly SharedActionsSystem _actions = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<RMCHunterNightVisionVisorComponent, ActivateVisorAttemptEvent>(OnAttempt);
        SubscribeLocalEvent<RMCHunterNightVisionVisorComponent, ActivateVisorEvent>(OnActivate);
        SubscribeLocalEvent<RMCHunterNightVisionVisorComponent, DeactivateVisorEvent>(OnDeactivate);
        SubscribeLocalEvent<RMCHunterNightVisionVisorComponent, VisorRelayedEvent<ScopedEvent>>(OnScoped);
        SubscribeLocalEvent<CycleableVisorComponent, GotUnequippedEvent>(OnCycleableUnequipped);
    }

    private void OnAttempt(Entity<RMCHunterNightVisionVisorComponent> ent, ref ActivateVisorAttemptEvent args)
    {
        if (!_container.TryGetContainingContainer(ent.Owner, out var container))
            return;

        if (_inventory.TryGetContainingSlot(container.Owner, out var slot) &&
            (slot.Name == "head" || slot.Name == "mask" || slot.Name == "eyes"))
        {
            return;
        }

        _popup.PopupClient("You must wear the mask to activate its modules.", args.User, args.User, PopupType.SmallCaution);
        args.Cancel();
    }

    private void OnCycleableUnequipped(Entity<CycleableVisorComponent> ent, ref GotUnequippedEvent args)
    {
        foreach (var containerId in ent.Comp.Containers)
        {
            if (!_container.TryGetContainer(ent, containerId, out var container))
                continue;

            foreach (var contained in container.ContainedEntities)
            {
                if (HasComp<RMCHunterNightVisionVisorComponent>(contained))
                {
                    _visor.DeactivateVisor(ent, contained, args.Equipee);
                    if (ent.Comp.Action is { } action)
                        _actions.SetIcon(action, ent.Comp.OffIcon);
                }
            }
        }
    }

    private void OnActivate(Entity<RMCHunterNightVisionVisorComponent> ent, ref ActivateVisorEvent args)
    {
        if (args.User != null && HasComp<ScopingComponent>(args.User))
        {
            _popup.PopupClient("You cannot use the night vision optic while using optics.",
                args.User.Value,
                args.User,
                PopupType.SmallCaution);
            return;
        }

        if (_timing.ApplyingState)
        {
            args.Handled = true;
            return;
        }

        if (args.User != null)
        {
            var user = args.User.Value;
            if (TryComp<NightVisionComponent>(user, out var nightVision))
            {
                nightVision.State = ent.Comp.State;
                nightVision.Green = false;
                nightVision.Mesons = true;
                nightVision.BlockScopes = true;
                Dirty(user, nightVision);
            }
            else
            {
                nightVision = new NightVisionComponent
                {
                    State = ent.Comp.State,
                    Green = false,
                    Mesons = true,
                    BlockScopes = true,
                };
                AddComp(user, nightVision, true);
            }

            _audio.PlayLocal(ent.Comp.SoundOn, ent, user);
            args.Handled = true;
        }
    }

    private void OnDeactivate(Entity<RMCHunterNightVisionVisorComponent> ent, ref DeactivateVisorEvent args)
    {
        if (_timing.ApplyingState)
            return;

        if (args.User != null)
        {
            var user = args.User.Value;
            if (TryComp<NightVisionComponent>(user, out var nightVision) && !nightVision.Innate)
            {
                RemCompDeferred<NightVisionComponent>(user);
            }

            _audio.PlayLocal(ent.Comp.SoundOff, ent, user);
        }
    }

    private void OnScoped(Entity<RMCHunterNightVisionVisorComponent> ent, ref VisorRelayedEvent<ScopedEvent> args)
    {
        if (args.Event.Scope.Comp.CanUseNightVision)
            return;

        _visor.DeactivateVisor(args.CycleableVisor, ent.Owner, args.Event.User);
    }
}
