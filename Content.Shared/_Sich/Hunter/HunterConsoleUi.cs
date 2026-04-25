using Robust.Shared.Serialization;

namespace Content.Shared._Sich.Hunter;

[Serializable, NetSerializable]
public enum HunterCrewUiKey : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class HunterConsoleState : BoundUserInterfaceState
{
    public readonly List<HunterTerminalData> Hunters;

    public HunterConsoleState(List<HunterTerminalData> hunters)
    {
        Hunters = hunters;
    }
}

[Serializable, NetSerializable]
public sealed record HunterTerminalData(string Name, string Status, string Position);
