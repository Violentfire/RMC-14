using Content.Shared.Inventory.Events;
using Content.Shared.Popups;
using Robust.Shared.GameObjects;
using Robust.Shared.Localization;

namespace Content.Shared._Sich.Marine.Squads;

public sealed class SichSherpaRiflemanSystem : EntitySystem
{
    [Dependency] private readonly SharedPopupSystem _popup = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<SichSherpaItemComponent, BeingEquippedAttemptEvent>(OnBeingEquippedAttempt);
    }

    private void OnBeingEquippedAttempt(Entity<SichSherpaItemComponent> item, ref BeingEquippedAttemptEvent args)
    {
        if (HasComp<SichSherpaRiflemanComponent>(args.EquipTarget))
            return;

        if (args.EquipTarget == args.Equipee)
            _popup.PopupClient(Loc.GetString("rmc-bulky-backpack-user-unable"), args.Equipee, args.Equipee, PopupType.MediumCaution);
        else
            _popup.PopupEntity(Loc.GetString("rmc-bulky-backpack-target-unable", ("target", args.EquipTarget)), args.Equipee, args.Equipee, PopupType.MediumCaution);

        args.Cancel();
    }
}
