using Robust.Shared.GameStates;
using Robust.Shared.Serialization;
using Robust.Shared.Prototypes;
using Content.Shared.Actions;

namespace Content.Shared._Sich.Hunter.Caster;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class HunterCasterComponent : Component
{
    [DataField, AutoNetworkedField]
    public HunterCasterMode CurrentMode = HunterCasterMode.Stun;

    [DataField, AutoNetworkedField]
    public bool Active = false;

    [DataField]
    public float FireDelay = 1.0f;
}

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class HunterCasterProviderComponent : Component
{
    [DataField]
    public EntProtoId ActionId = "ActionToggleHunterCaster";

    [DataField, AutoNetworkedField]
    public bool Active = false;

    [DataField, AutoNetworkedField]
    public EntityUid? SpawnedCaster;

    public EntityUid? ActionEntity;
}

[Serializable, NetSerializable]
public enum HunterCasterMode : byte
{
    Stun,
    Immobilizer,
    Bolt,
    Eradicator
}

[Serializable, NetSerializable]
public enum HunterCasterVisualLayers : byte
{
    Base
}

public sealed partial class HunterCasterActionEvent : InstantActionEvent {}
