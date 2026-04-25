using Content.Shared.Actions;
using Robust.Shared.Serialization;

namespace Content.Shared._Sich.Language;

/// <summary>
///     Компонент-маркер для мобів, які розмовляють мисливською мовою.
/// </summary>
[RegisterComponent]
public sealed partial class HunterLanguageComponent : Component
{
    /// <summary>
    ///     Чи розуміє цей моб звичайну людську мову?
    ///     За замовчуванням так для мисливців.
    /// </summary>
    [DataField("understandsHuman")]
    public bool UnderstandsHuman = true;

    /// <summary>
    ///     Який префікс використовувати для обфускованих повідомлень.
    /// </summary>
    [DataField("obfuscationPrefix")]
    public string ObfuscationPrefix = "cm-hunter-speech-obfuscated-";
}

/// <summary>
///     Компонент для браслетів-перекладачів.
/// </summary>
[RegisterComponent]
public sealed partial class HunterTranslatorComponent : Component
{
    /// <summary>
    ///     Максимальна довжина повідомлення для перекладу.
    /// </summary>
    [DataField("maxLength")]
    public int MaxLength = 200;

    [DataField("actionId")]
    public string ActionId = "HunterActionTranslator";

    [DataField("actionEntity")]
    public EntityUid? ActionEntity;
}

[RegisterComponent]
public sealed partial class HunterTranslatingMessageComponent : Component {}

[Serializable, NetSerializable]
public enum HunterTranslatorUiKey : byte
{
    Key
}

[Serializable, NetSerializable]
public sealed class HunterTranslatorSendMessage : BoundUserInterfaceMessage
{
    public string Message { get; }

    public HunterTranslatorSendMessage(string message)
    {
        Message = message;
    }
}

/// <summary>
///     Подія активації актіона перекладача.
/// </summary>
public sealed partial class HunterTranslatorActionEvent : InstantActionEvent {}
