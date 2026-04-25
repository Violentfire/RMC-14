using Content.Client.UserInterface.Systems.Actions.Widgets;
using Content.Client.UserInterface.Systems.Actions.Controls;
using Content.Shared._RMC14.Actions.Components;
using Content.Shared.Actions.Components;
using Robust.Client.GameObjects;
using Robust.Client.UserInterface;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Timing;
using System.Numerics;

namespace Content.Client._RMC14.Actions;

public sealed class RMCActionBackgroundSystem : EntitySystem
{
    [Dependency] private readonly IUserInterfaceManager _ui = default!;
    [Dependency] private readonly SpriteSystem _sprite = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (_ui.GetActiveUIWidgetOrNull<ActionsBar>() is not { ActionsContainer: { } container })
            return;

        foreach (var button in container.GetButtons())
        {
            UpdateActionBackground(button);
        }
    }

    private void UpdateActionBackground(ActionButton button)
    {
        var actionId = button.Action;
        bool hasComp = actionId != null && HasComp<RMCActionBackgroundComponent>(actionId.Value);

        // Find or create our custom background
        TextureRect? customBg = null;
        foreach (var child in button.Children)
        {
            if (child.Name == "RMCActionBackground")
            {
                customBg = child as TextureRect;
                break;
            }
        }

        if (!hasComp)
        {
            if (customBg != null)
            {
                button.RemoveChild(customBg);
                button.Button.Visible = true;
            }
            return;
        }

        // We have the component
        if (customBg == null)
        {
            customBg = new TextureRect
            {
                Name = "RMCActionBackground",
                MouseFilter = Control.MouseFilterMode.Ignore,
                Stretch = TextureRect.StretchMode.Scale,
                MinSize = new Vector2(64, 64),
            };
            button.AddChild(customBg);
            customBg.SetPositionInParent(1); // After the original Button (index 0)
            button.Button.Visible = false;
        }

        if (actionId == null ||
            !TryComp<ActionComponent>(actionId.Value, out var action) ||
            !TryComp<RMCActionBackgroundComponent>(actionId.Value, out var backgroundComp))
        {
            return;
        }

        var background = backgroundComp.Background;
        if (action.Toggled && backgroundComp.BackgroundOn != null)
            background = backgroundComp.BackgroundOn;

        if (background != null)
            customBg.Texture = _sprite.Frame0(background);
        else
            customBg.Texture = null;
    }
}
