using Content.Shared._Sich.Language;
using Content.Shared.Actions;
using Content.Shared.Inventory.Events;
using Content.Shared.Chat;
using Content.Server.Chat.Systems;
using Robust.Server.GameObjects;

namespace Content.Server._Sich.Language;

/// <summary>
///     Система, що обробляє браслети-перекладачі та їхній актіон.
/// </summary>
public sealed class HunterTranslatorSystem : EntitySystem
{
    [Dependency] private readonly ChatSystem _chat = default!;
    [Dependency] private readonly SharedActionsSystem _actions = default!;
    [Dependency] private readonly UserInterfaceSystem _ui = default!;

    public override void Initialize()
    {
        base.Initialize();
        
        SubscribeLocalEvent<HunterTranslatorComponent, GotEquippedEvent>(OnEquipped);
        SubscribeLocalEvent<HunterTranslatorComponent, GotUnequippedEvent>(OnUnequipped);
        
        // Обробка активації актіона
        SubscribeLocalEvent<HunterTranslatorComponent, HunterTranslatorActionEvent>(OnActionActivated);
        
        // Обробка повідомлення з UI
        SubscribeLocalEvent<HunterTranslatorComponent, HunterTranslatorSendMessage>(OnSendMessage);
    }

    private void OnEquipped(EntityUid uid, HunterTranslatorComponent component, GotEquippedEvent args)
    {
        // Надаємо актіон лише якщо вдягнуто в правильний слот (зазвичай Hands)
        // Але оскільки ми хочемо "браслети", перевіримо слот
        if (args.Slot != "gloves") // В RMC-14 браслети часто в слоті рукавиць
            return;

        _actions.AddAction(args.Equipee, ref component.ActionEntity, component.ActionId, uid);
    }

    private void OnUnequipped(EntityUid uid, HunterTranslatorComponent component, GotUnequippedEvent args)
    {
        _actions.RemoveAction(args.Equipee, component.ActionEntity);
    }

    private void OnActionActivated(EntityUid uid, HunterTranslatorComponent component, HunterTranslatorActionEvent args)
    {
        if (args.Handled)
            return;

        // Відкриваємо UI для того, хто вдягнув браслети
        _ui.OpenUi(uid, HunterTranslatorUiKey.Key, args.Performer);
        args.Handled = true;
    }

    private void OnSendMessage(EntityUid uid, HunterTranslatorComponent component, HunterTranslatorSendMessage args)
    {
        var sender = args.Actor;
        if (sender == default)
            return;

        // Обрізаємо повідомлення, якщо воно занадто довге
        var message = args.Message;
        if (message.Length > component.MaxLength)
            message = message[..component.MaxLength];

        // Тимчасово додаємо маркер, щоб обійти обфускацію
        AddComp<HunterTranslatingMessageComponent>(sender);

        try
        {
            // Відправляємо повідомлення в чат
            _chat.TrySendInGameICMessage(sender, message, InGameICChatType.Speak, false);
        }
        finally
        {
            // ЗАВЖДИ видаляємо маркер після відправки, навіть якщо сталася помилка
            RemCompDeferred<HunterTranslatingMessageComponent>(sender);
        }
    }
}

