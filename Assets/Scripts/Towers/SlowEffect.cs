/// <summary>
///     Overtime effect, that slows target <see cref="Enemy" />>.
/// </summary>
public class SlowEffect : Effect<Enemy>
{
    private readonly float multiplier;
    private float elapsed;

    public SlowEffect(
        Enemy enemy,
        float multiplier,
        float duration,
        float interval) : base(enemy, duration, interval)
    {
        this.multiplier = multiplier;
    }

    // TODO: Fix the issue where it does not find effect controller, but still applies slow.
    // For now that should not happen as I have structured it this way.
    public override void Apply()
    {
        base.Apply();
        target.moveSpeedMultiplier *= multiplier;
    }

    public override void StopEffect()
    {
        base.StopEffect();
        target.moveSpeedMultiplier /= multiplier;
    }

    public override void TriggerEffect() { }
}