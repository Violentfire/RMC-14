using Content.Shared._RMC14.Hunter.Systems;
using Content.Shared._RMC14.NightVision;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;

namespace Content.Shared._RMC14.Hunter.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
[Access(typeof(RMCHunterNightVisionSystem))]
public sealed partial class RMCHunterNightVisionVisorComponent : Component
{
    [DataField, AutoNetworkedField]
    public SoundSpecifier? SoundOn = new SoundPathSpecifier("/Audio/_RMC14/Handling/toggle_nv1.ogg");

    [DataField, AutoNetworkedField]
    public SoundSpecifier? SoundOff = new SoundPathSpecifier("/Audio/_RMC14/Handling/toggle_nv2.ogg");

    [DataField, AutoNetworkedField]
    public NightVisionState State = NightVisionState.Half;
}
