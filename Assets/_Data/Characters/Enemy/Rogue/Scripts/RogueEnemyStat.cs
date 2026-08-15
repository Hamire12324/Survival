public class RogueEnemyStat : EnemyStat
{
    protected override void ResetValue()
    {
        base.ResetValue();
        MaxHealth = 180f;
        MoveSpeed = 2.7f;
    }
}
