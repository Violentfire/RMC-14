using Robust.Shared.GameStates;

namespace Content.Shared._Sich.Hunter;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class HunterComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool AutoTracked = false;
}
