using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Content.Shared.DoAfter;

namespace Content.Shared._Sich.Hunter.Caster;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class HunterBracerComponent : Component
{
    [DataField, AutoNetworkedField]
    public float RechargeMultiplierShip = 50f;

    [DataField, AutoNetworkedField]
    public float RechargeMultiplierGround = 1f;
}

[RegisterComponent, NetworkedComponent]
public sealed partial class HunterShipComponent : Component
{
}

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SichApcSiphonedComponent : Component
{
    [DataField, AutoNetworkedField]
    public int SiphonCount = 0;

    [DataField, AutoNetworkedField]
    public TimeSpan SiphonCooldown = TimeSpan.FromMinutes(5);

    [DataField, AutoNetworkedField]
    public TimeSpan LastSiphonTime = TimeSpan.Zero;
}

[Serializable, NetSerializable]
public sealed partial class HunterSiphonDoAfterEvent : SimpleDoAfterEvent
{
}
