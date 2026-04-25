using Content.Shared.Actions;
using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared._RMC14.Actions.Components;

[RegisterComponent, NetworkedComponent]
[AutoGenerateComponentState]
public sealed partial class RMCActionBackgroundComponent : Component
{
    [DataField, AutoNetworkedField]
    public SpriteSpecifier? Background;

    [DataField, AutoNetworkedField]
    public SpriteSpecifier? BackgroundOn;
}
