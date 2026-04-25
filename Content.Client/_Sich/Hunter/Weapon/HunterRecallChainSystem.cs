using System.Numerics;
using Content.Shared._Sich.Hunter.Weapon;
using Robust.Client.GameObjects;

namespace Content.Client._Sich.Hunter.Weapon;

public sealed class HunterRecallChainSystem : EntitySystem
{
    [Dependency] private readonly SharedTransformSystem _transform = default!;
    [Dependency] private readonly SpriteSystem _sprite = default!;

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<HunterRecallChainComponent, SpriteComponent>();
        while (query.MoveNext(out var uid, out var chain, out var sprite))
        {
            if (!Exists(chain.Source) || !Exists(chain.Target))
                continue;

            var sourcePos = _transform.GetWorldPosition(chain.Source);
            var targetPos = _transform.GetWorldPosition(chain.Target);

            var diff = targetPos - sourcePos;
            var length = diff.Length();
            
            if (length < 0.1f)
            {
                _sprite.SetVisible((uid, sprite), false);
                continue;
            }

            _sprite.SetVisible((uid, sprite), true);
            var angle = diff.ToWorldAngle();
            var midpoint = sourcePos + diff / 2f;

            _transform.SetWorldPosition(uid, midpoint);
            _transform.SetWorldRotation(uid, angle);
            
            // We scale the length (Y axis for ToWorldAngle oriented sprites usually)
            // or X axis if the sprite is horizontal. 
            // In SS14, ToWorldAngle() 0 is North. If the sprite is a vertical strip, 
            // then we scale Y. If it's horizontal, we scale X but then rotation might be off.
            // Let's try scaling Y as length and X as width.
            _sprite.SetScale((uid, sprite), new Vector2(0.4f, length));
        }
    }
}
