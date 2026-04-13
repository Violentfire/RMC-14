// Зроблено POMAHtop (ДС). Питання по тому, як це працює краще задавайте йому :godo:
using Content.Server.Speech.Components;
using Content.Shared.Speech;
using Robust.Shared.Random;

namespace Content.Server.Speech.EntitySystems;

public sealed class ArachnidAccentSystem : EntitySystem
{
    [Dependency] private readonly IRobustRandom _random = default!;
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ArachnidAccentComponent, AccentGetEvent>(OnAccent);
    }

    private void OnAccent(EntityUid uid, ArachnidAccentComponent component, AccentGetEvent args)
    {
        var message = args.Message;
        if (string.IsNullOrEmpty(message)) return;

        ReadOnlySpan<char> ispan = message.AsSpan();
        int messageSeed = _random.Next(0, int.MaxValue);

        const uint chance30 = 644245094;
        const uint chance50 = 1073741823;

        var budgetRng = new FastRandom(messageSeed);
        uint endingRoll = budgetRng.NextUint();

        bool hasEnding = endingRoll < chance50;

        int extraSpace = hasEnding ? 3 : 0;

        int boundary = ispan.Length;
        for (int i = ispan.Length - 1; i >= 0; i--)
        {
            if (char.IsLetter(ispan[i]))
            {
                boundary = i + 1;
                break;
            }
        }
        for (int i = 0; i < boundary; i++)
        {
            char c = ispan[i];
            if (c == 'ц' || c == 'ч' || c == 'щ' || c == 'Ц' || c == 'Ч' || c == 'Щ')
            {
                if (budgetRng.NextUint() < chance30) extraSpace++;
            }
        }


        int finalLength = ispan.Length + extraSpace;
        args.Message = string.Create(finalLength, (message, boundary, hasEnding, endingRoll, messageSeed), (dest, state) =>
        {
            var (original, bnd, addEnding, endRoll, seed) = state;
            var writeRng = new FastRandom(seed);
            // Синхронізуйте з budgetRng.NextUint() для endingRoll
            writeRng.NextUint();

            int writeIdx = 0;

            for (int i = 0; i < bnd; i++)
            {
                char c = original[i];
                dest[writeIdx++] = c;

                if (c == 'ц' || c == 'ч' || c == 'щ' || c == 'Ц' || c == 'Ч' || c == 'Щ')
                {
                    if (writeRng.NextUint() < chance30) dest[writeIdx++] = c;
                }
            }

            if (addEnding)
            {
                ReadOnlySpan<char> suffix = ((endRoll & 1) == 0) ? "-тц" : "-кх";
                suffix.CopyTo(dest.Slice(writeIdx));
                writeIdx += 3;
            }

            original.AsSpan().Slice(bnd).CopyTo(dest.Slice(writeIdx));
        });
    }
    private struct FastRandom
    {
        private uint _state;

        public FastRandom(int seed)
        {
            _state = seed == 0 ? 123456789u : (uint)seed;
        }

        public uint NextUint()
        {
            _state = (_state * 1103515245u + 12345u) & 0x7FFFFFFF;
            return _state;
        }
    }
}
