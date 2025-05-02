using System.Text.RegularExpressions;
using Content.Server.Speech.Components;

namespace Content.Server.Speech.EntitySystems;

public sealed class LizardAccentSystem : EntitySystem
{
    private static readonly Regex RegexLowerS = new("с+");
    private static readonly Regex RegexUpperS = new("С+");
    private static readonly Regex RegexLowerSh = new("ш+");
    private static readonly Regex RegexUpperSh = new("Ш+");
    private static readonly Regex RegexLowerShch = new("щ+");
    private static readonly Regex RegexUpperShch = new("Щ+");
    private static readonly Regex RegexLowerZh = new("ж+");
    private static readonly Regex RegexUpperZh = new("Ж+");

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<LizardAccentComponent, AccentGetEvent>(OnAccent);
    }

    private void OnAccent(EntityUid uid, LizardAccentComponent component, AccentGetEvent args)
    {
        var message = args.Message;

        // hissss
        message = RegexLowerS.Replace(message, "ссс");
        // hiSSS
        message = RegexUpperS.Replace(message, "ССС");
        // lower SH
        message = RegexLowerSh.Replace(message, "шшш");
        // upper SH
        message = RegexUpperSh.Replace(message, "ШШШ");
        // lower SHCH
        message = RegexLowerShch.Replace(message, "шшщ");
        // upper SHCH
        message = RegexUpperShch.Replace(message, "ШШЩ");
        // lower ZH
        message = RegexLowerZh.Replace(message, "жшш");
        //upper ZH
        message = RegexUpperZh.Replace(message. "ЖШШ");

        args.Message = message;
    }
}
