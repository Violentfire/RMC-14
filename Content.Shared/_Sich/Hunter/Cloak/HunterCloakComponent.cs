using Content.Shared.FixedPoint;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared._Sich.Hunter.Cloak;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class HunterCloakComponent : Component
{
    [DataField, AutoNetworkedField]
    public bool Enabled;

    [DataField, AutoNetworkedField]
    public float Opacity = 0.1f;

    [DataField, AutoNetworkedField]
    public FixedPoint2 EnergyCost = 50;

    [DataField, AutoNetworkedField]
    public FixedPoint2 MinEnergy = 50;

    [DataField, AutoNetworkedField]
    public float PassiveEnergyCost = 2f; // per second

    [DataField, AutoNetworkedField]
    public float FlickerDuration = 0.5f;

    [DataField, AutoNetworkedField]
    public float FlickerOpacity = 0.5f;

    [DataField, AutoNetworkedField]
    public float DamageBreakThreshold = 10f;

    [DataField, AutoNetworkedField]
    public EntProtoId ActionId = "ActionToggleHunterCloak";

    [DataField, AutoNetworkedField]
    public EntityUid? Action;

    [DataField, AutoNetworkedField]
    public SoundSpecifier CloakSound = new SoundPathSpecifier("/Audio/_Sich/Effects/pred_cloakon.ogg");

    [DataField, AutoNetworkedField]
    public SoundSpecifier UncloakSound = new SoundPathSpecifier("/Audio/_Sich/Effects/pred_cloakoff.ogg");

    [DataField, AutoNetworkedField]
    public EntProtoId CloakEffect = "RMCEffectCloak";

    [DataField, AutoNetworkedField]
    public EntProtoId UncloakEffect = "RMCEffectUncloak";
}
