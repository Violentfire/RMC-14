using System.Text.RegularExpressions;
using Content.Server.Speech.Components;
using Robust.Shared.Random;

namespace Content.Server.Speech.EntitySystems;

public sealed class FelinidAccentSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    private static readonly Regex RegexLowerMa = new("ма+");
    private static readonly Regex RegexUpperMa = new("МА+");
    private static readonly Regex RegexLowerNa = new("на+");
    private static readonly Regex RegexUpperNa = new("НА+");
    private static readonly Regex RegexLowerNe = new("не+");
    private static readonly Regex RegexUpperNe = new("НЕ+");

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<FelinidAccentComponent, AccentGetEvent>(OnAccent);
    }

    public string AddEndingNyaw(string message)
    {
        var msg = message;
        var ending = "";
        char last_letter = '.';
        int i = 1;
        while(i < msg.Length)
            {
                if(char.IsLetter(msg[^i]))
                {
                    last_letter = msg[^i];
                    break;
                }
                i++;
            }
        if(message.Length > 5)
        {
            if(_random.Prob(0.1f))
            {
                int choice = 0;
                if(!string.IsNullOrEmpty(msg) && char.IsUpper(last_letter))
                {
                    choice = _random.Next(5);
                }
                else
                {
                    choice = _random.Next(7);
                }


                switch(choice)
                {
                    case 0:
                        ending =" -няв";
                        break;
                    case 1:
                        ending = " -мяв";
                        break;
                    case 2:
                        ending = " -мав";
                        break;
                    case 3:
                        ending =  " -мя";
                        break;
                    case 4:
                        ending =  " -ня";
                        break;
                    case 5:
                        ending = " -пурр";
                        break;
                    case 6:
                        ending = " -мурр";
                        break;
                }

            }
        }

        if (!string.IsNullOrEmpty(msg) && char.IsUpper(last_letter))
        {
            ending = ending.ToUpper();
        }

        return msg + ending;
    }

    private void OnAccent(EntityUid uid, FelinidAccentComponent component, AccentGetEvent args)
    {
        var message = args.Message;

        message = RegexLowerMa.Replace(message, "мя");
        message = RegexUpperMa.Replace(message, "МЯ");
        message = RegexLowerNa.Replace(message, "ня");
        message = RegexUpperNa.Replace(message, "НЯ");
        message = RegexLowerNe.Replace(message, "ня");
        message = RegexUpperNe.Replace(message, "НЯ");


        args.Message = AddEndingNyaw(message);
    }
}
