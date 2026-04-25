using Content.Shared._Sich.Hunter;
using JetBrains.Annotations;
using Robust.Client.GameObjects;

namespace Content.Client._Sich.Hunter;

[UsedImplicitly]
public sealed class HunterCrewBui : BoundUserInterface
{
    private HunterCrewWindow? _window;

    public HunterCrewBui(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();
        _window = new HunterCrewWindow();
        _window.OnClose += Close;
        _window.OpenCentered();
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        base.UpdateState(state);
        if (state is HunterConsoleState hunterState)
            _window?.UpdateState(hunterState);
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
            _window?.Dispose();
    }
}
