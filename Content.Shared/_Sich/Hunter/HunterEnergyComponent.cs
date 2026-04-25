using Content.Shared.FixedPoint;
using Robust.Shared.GameStates;
using Content.Shared.Alert;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype;

namespace Content.Shared._Sich.Hunter;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class HunterEnergyComponent : Component
{
    [DataField, AutoNetworkedField]
    public FixedPoint2 Energy = 1000;

    [DataField, AutoNetworkedField]
    public FixedPoint2 MaxEnergy = 1000;

    [DataField, AutoNetworkedField]
    public FixedPoint2 EnergyRegen = 10; // per second (Ground)

    [DataField, AutoNetworkedField]
    public FixedPoint2 EnergyRegenShip = 25; // per second (on Hunter Ship)

    [DataField(customTypeSerializer: typeof(PrototypeIdSerializer<AlertPrototype>))]
    public string Alert = "HunterBattery";
}

[RegisterComponent, NetworkedComponent]
public sealed partial class HunterActionEnergyComponent : Component
{
    [DataField]
    public FixedPoint2 Cost = 0;
}
