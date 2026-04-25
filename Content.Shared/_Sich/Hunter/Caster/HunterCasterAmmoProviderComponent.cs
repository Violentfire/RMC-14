using Content.Shared.Weapons.Ranged.Components;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared._Sich.Hunter.Caster;

/// <summary>
/// This component replaces the standard battery ammo provider.
/// It draws power from a HunterBracer battery on the wearer.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class HunterCasterAmmoProviderComponent : BatteryAmmoProviderComponent
{
    [DataField, ViewVariables(VVAccess.ReadWrite)]
    public EntProtoId? Prototype;
}

[Serializable, NetSerializable]
public sealed class HunterCasterAmmoProviderComponentState : ComponentState
{
    public int Shots;
    public int Capacity;
    public float FireCost;

    public HunterCasterAmmoProviderComponentState(int shots, int capacity, float fireCost)
    {
        Shots = shots;
        Capacity = capacity;
        FireCost = fireCost;
    }
}
