using Robust.Shared.GameStates;
using Robust.Shared.Serialization;

namespace Content.Shared._RMC14.Hunter.Components;

[RegisterComponent, NetworkedComponent]
public sealed partial class HunterClawsComponent : Component
{
    [DataField]
    public EntityUid ModuleEntity;
}
