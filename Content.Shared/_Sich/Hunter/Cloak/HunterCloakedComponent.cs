using Robust.Shared.GameStates;

namespace Content.Shared._Sich.Hunter.Cloak;

[RegisterComponent, NetworkedComponent]
public sealed partial class HunterCloakedComponent : Component
{
    public TimeSpan FlickerUntil;
}
