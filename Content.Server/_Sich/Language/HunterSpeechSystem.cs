using Content.Server._RMC14.Chat.Chat;
using Content.Shared._Sich.Language;
using Robust.Shared.Player;

namespace Content.Server._Sich.Language;

/// <summary>
///     Система, що приховує мову мисливців від інших видів.
/// </summary>
public sealed class HunterSpeechSystem : EntitySystem
{
    private readonly List<ICommonSession> _toRemove = new();

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<HunterLanguageComponent, ChatMessageAfterGetRecipients>(OnHunterAfterGetRecipients);
    }

    private void OnHunterAfterGetRecipients(EntityUid uid, HunterLanguageComponent component, ref ChatMessageAfterGetRecipients args)
    {
        // Якщо працює перекладач - повідомлення чують всі
        if (HasComp<HunterTranslatingMessageComponent>(uid))
            return;

        _toRemove.Clear();

        foreach (var (session, data) in args.Recipients)
        {
            // Спостерігачі бачать все
            if (data.Observer)
                continue;

            // Ховаємо повідомлення від тих, хто не знає мови мисливців
            if (!HasComp<HunterLanguageComponent>(session.AttachedEntity))
                _toRemove.Add(session);
        }

        foreach (var session in _toRemove)
        {
            args.Recipients.Remove(session);
        }
    }
}
