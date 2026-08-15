public class EnemyStat : CharacterStat
{
    protected override void ResetValue()
    {
        base.ResetValue();
        MaxHealth = 220f;
        MoveSpeed = 3f;
        RotationSpeed = 180f;
        Armor = 0f;
        DamageMultiplier = 0f;
    }
}
