using Content.Shared._Sich.Language;
using Robust.Client.GameObjects;

namespace Content.Client._Sich.Language.UI;

public sealed class HunterTranslatorBoundUserInterface : BoundUserInterface
{
    private HunterTranslatorWindow? _window;

    public HunterTranslatorBoundUserInterface(EntityUid owner, Enum uiKey) : base(owner, uiKey)
    {
    }

    protected override void Open()
    {
        base.Open();

        _window = new HunterTranslatorWindow();
        _window.OnClose += Close;
        _window.OnSendMessage += message =>
        {
            SendMessage(new HunterTranslatorSendMessage(message));
        };

        _window.OpenCentered();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);
        if (disposing)
        {
            _window?.Dispose();
        }
    }
}
