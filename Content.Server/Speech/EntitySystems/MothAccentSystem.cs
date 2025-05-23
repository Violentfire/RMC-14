﻿using System.Text.RegularExpressions;
using Content.Server.Speech.Components;

namespace Content.Server.Speech.EntitySystems;

public sealed class MothAccentSystem : EntitySystem
{
    private static readonly Regex RegexLowerBuzz = new Regex("з{1,3}");
    private static readonly Regex RegexUpperBuzz = new Regex("З{1,3}");
    private static readonly Regex RegexLowerBuzh = new Regex("ж{1,3}");
    private static readonly Regex RegexUpperBuzh = new Regex("Ж{1,3}");
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<MothAccentComponent, AccentGetEvent>(OnAccent);
    }

    private void OnAccent(EntityUid uid, MothAccentComponent component, AccentGetEvent args)
    {
        var message = args.Message;

        // buzzz
        message = RegexLowerBuzz.Replace(message, "ззз");
        // buZZZ
        message = RegexUpperBuzz.Replace(message, "ЗЗЗ");
        // buzzh
        message = RegexLowerBuzh.Replace(message, "жжж");
        // buZZH
        message = RegexUpperBuzh.Replace(message, "ЖЖЖ");

        args.Message = message;
    }
}
