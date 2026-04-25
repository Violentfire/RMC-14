using Robust.Shared.GameObjects;

namespace Content.Shared._Sich.Hunter.Weapon;

public sealed class SharedHunterRecallChainSystem : EntitySystem
{
    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<HunterRecallChainComponent>();
        while (query.MoveNext(out var uid, out var chain))
        {
            if (!Exists(chain.Source) || !Exists(chain.Target))
            {
                QueueDel(uid);
            }
        }
    }
}
