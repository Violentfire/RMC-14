using Robust.Shared.GameStates;

namespace Content.Shared._Sich.Hunter.Weapon;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class HunterRecallChainComponent : Component
{
    [DataField, AutoNetworkedField]
    public EntityUid Source;

    [DataField, AutoNetworkedField]
    public EntityUid Target;
}
