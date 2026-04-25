using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._Sich.Hunter.Weapon;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class HunterRecallWeaponComponent : Component
{
    [DataField, AutoNetworkedField]
    public EntityUid? BoundOwner;

    [DataField, AutoNetworkedField]
    public EntityUid? ChainEntity;

    [DataField, AutoNetworkedField]
    public float MaxRecallRange = 10f;

    [DataField, AutoNetworkedField]
    public float RecallCost = 50f;
}
