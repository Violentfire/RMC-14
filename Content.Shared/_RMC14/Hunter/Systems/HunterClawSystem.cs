using Content.Shared._RMC14.Hands;
using Content.Shared._RMC14.Hunter.Components;
using Content.Shared.Actions;
using Content.Shared.Actions.Components;
using Content.Shared.Hands.Components;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Inventory;
using Content.Shared.Inventory.Events;
using Content.Shared.Interaction.Events;
using Content.Shared.Hands;
using Content.Shared.Containers.ItemSlots;
using Content.Shared._RMC14.Hunter.Events;
using Robust.Shared.Containers;
using Robust.Shared.Network;

namespace Content.Shared._RMC14.Hunter.Systems;

public sealed class HunterClawSystem : EntitySystem
{
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly ActionContainerSystem _actionsContainer = default!;
    [Dependency] private readonly SharedContainerSystem _container = default!;
    [Dependency] private readonly ItemSlotsSystem _itemSlots = default!;
    [Dependency] private readonly SharedHandsSystem _hands = default!;
    [Dependency] private readonly InventorySystem _inventory = default!;
    [Dependency] private readonly INetManager _net = default!;

    public override void Initialize()
    {
        SubscribeLocalEvent<HunterClawModuleComponent, ComponentShutdown>(OnModuleShutdown);
        SubscribeLocalEvent<HunterClawModuleComponent, HunterClawsToggleActionEvent>(OnToggleClaws);

        SubscribeLocalEvent<HunterBraceletsComponent, GetItemActionsEvent>(OnBraceletsGetItemActions);
        SubscribeLocalEvent<HunterBraceletsComponent, HunterClawsToggleActionEvent>(OnBraceletsToggleClaws);
        SubscribeLocalEvent<HunterBraceletsComponent, GotUnequippedEvent>(OnBraceletsUnequipped);
        SubscribeLocalEvent<HunterBraceletsComponent, EntInsertedIntoContainerMessage>(OnBraceletsContainerChanged);
        SubscribeLocalEvent<HunterBraceletsComponent, EntRemovedFromContainerMessage>(OnBraceletsContainerChanged);

        SubscribeLocalEvent<HunterClawsComponent, RMCItemDropAttemptEvent>(OnClawDropAttempt);
        SubscribeLocalEvent<HunterClawsComponent, BeingUnequippedAttemptEvent>(OnClawUnequipAttempt);
    }

    private void OnBraceletsContainerChanged(EntityUid uid, HunterBraceletsComponent component, ContainerModifiedMessage args)
    {
        if (args.Container.ID != "module_slot")
            return;

        if (args is EntRemovedFromContainerMessage removed && 
            TryComp<HunterClawModuleComponent>(removed.Entity, out var moduleComp))
        {
            RetractClaws(removed.Entity, moduleComp);
        }

        RefreshBracelets(uid);
    }

    private void RefreshBracelets(EntityUid uid)
    {
        if (_net.IsClient)
            return;

        if (!_container.TryGetContainingContainer(uid, out var container))
            return;

        var wearer = container.Owner;
        if (!HasComp<Content.Shared.Actions.Components.ActionsComponent>(wearer))
            return;

        _actions.RemoveProvidedActions(wearer, uid);

        if (!_inventory.TryGetContainingSlot(uid, out var slot) || slot.Name != "gloves")
            return;

        var ev = new Content.Shared.Actions.GetItemActionsEvent(_actionsContainer, wearer, uid, slot.SlotFlags);
        RaiseLocalEvent(uid, ev);

        if (ev.Actions.Count > 0)
        {
            _actions.GrantActions(wearer, ev.Actions, uid);
        }
    }

    private void OnBraceletsGetItemActions(EntityUid uid, HunterBraceletsComponent component, ref GetItemActionsEvent args)
    {
        if (!_inventory.TryGetContainingSlot(uid, out var inventorySlot) || inventorySlot.Name != "gloves")
            return;

        if (_itemSlots.TryGetSlot(uid, "module_slot", out var slot) && slot.HasItem)
        {
            if (TryComp<HunterClawModuleComponent>(slot.Item, out var moduleComp))
            {
                args.AddAction(ref moduleComp.ToggleActionEntity, moduleComp.ToggleAction, uid);
            }
        }
    }

    private void OnBraceletsToggleClaws(EntityUid uid, HunterBraceletsComponent component, HunterClawsToggleActionEvent args)
    {
        if (args.Handled)
            return;

        if (!_inventory.TryGetContainingSlot(uid, out var inventorySlot) || inventorySlot.Name != "gloves")
            return;

        if (_itemSlots.TryGetSlot(uid, "module_slot", out var slot) && slot.HasItem)
        {
            if (TryComp<HunterClawModuleComponent>(slot.Item, out var moduleComp))
            {
                OnToggleClaws(slot.Item.Value, moduleComp, args);
            }
        }
    }

    private void OnBraceletsUnequipped(EntityUid uid, HunterBraceletsComponent component, GotUnequippedEvent args)
    {
        if (_itemSlots.TryGetSlot(uid, "module_slot", out var slot) && slot.HasItem)
        {
            if (TryComp<HunterClawModuleComponent>(slot.Item, out var moduleComp))
            {
                RetractClaws(slot.Item.Value, moduleComp);
            }
        }
    }

    private void OnModuleShutdown(EntityUid uid, HunterClawModuleComponent component, ComponentShutdown args)
    {
        RetractClaws(uid, component);
        _actions.RemoveAction(uid, component.ToggleActionEntity);
    }

    private void OnToggleClaws(EntityUid uid, HunterClawModuleComponent component, HunterClawsToggleActionEvent args)
    {
        if (args.Handled)
            return;

        var xform = Transform(uid);
        if (!TryComp<HunterBraceletsComponent>(xform.ParentUid, out _))
            return;

        if (!_inventory.TryGetContainingSlot(xform.ParentUid, out var inventorySlot) || inventorySlot.Name != "gloves")
            return;

        if (component.ActiveClaws.Count > 0)
        {
            RetractClaws(uid, component);
        }
        else
        {
            DeployClaws(uid, component, args.Performer);
        }

        args.Handled = true;
    }

    private void DeployClaws(EntityUid uid, HunterClawModuleComponent component, EntityUid user)
    {
        if (_net.IsClient)
            return;

        if (!TryComp<HandsComponent>(user, out var handsComp))
            return;

        var emptyHands = new List<string>();
        foreach (var (handName, _) in handsComp.Hands)
        {
            if (_hands.GetHeldItem(user, handName) == null)
            {
                emptyHands.Add(handName);
            }
        }

        if (emptyHands.Count == 0)
        {
            return;
        }

        foreach (var handName in emptyHands)
        {
            var claw = Spawn(component.ClawPrototype, Transform(user).Coordinates);
            var clawComp = EnsureComp<HunterClawsComponent>(claw);
            clawComp.ModuleEntity = uid;
            
            if (_hands.TryPickup(user, claw, handName, handsComp: handsComp))
            {
                component.ActiveClaws.Add(claw);
            }
            else
            {
                QueueDel(claw);
            }
        }
    }

    private void RetractClaws(EntityUid uid, HunterClawModuleComponent component)
    {
        if (_net.IsClient)
            return;

        foreach (var claw in component.ActiveClaws.ToArray())
        {
            if (TerminatingOrDeleted(claw))
                continue;

            QueueDel(claw);
        }

        component.ActiveClaws.Clear();
    }

    private void OnClawDropAttempt(EntityUid uid, HunterClawsComponent component, ref RMCItemDropAttemptEvent args)
    {
        args.Cancelled = true;
    }

    private void OnClawUnequipAttempt(EntityUid uid, HunterClawsComponent component, BeingUnequippedAttemptEvent args)
    {
        args.Cancel();
    }
}
