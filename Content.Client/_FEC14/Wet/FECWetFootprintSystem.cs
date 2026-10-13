using Content.Shared._FEC14.Wet;
using Robust.Client.GameObjects;
using Robust.Shared.Timing;

namespace Content.Client._FEC14.Wet;

public sealed class FECWetFootprintSystem : EntitySystem
{
    [Dependency] private readonly SpriteSystem _sprite = default!;
    [Dependency] private readonly IGameTiming _timing = default!;

    public override void FrameUpdate(float frameTime)
    {
        var now = _timing.CurTime;
        var query = EntityQueryEnumerator<FECWetFootprintComponent, SpriteComponent>();
        while (query.MoveNext(out var uid, out var print, out var sprite))
        {
            if (print.Lifetime <= TimeSpan.Zero)
                continue;

            var alpha = (float) Math.Clamp((print.ExpiresAt - now) / print.Lifetime, 0.0, 1.0);
            _sprite.SetColor((uid, sprite), Color.White.WithAlpha(alpha));
        }
    }
}
