using UnityEngine;

public class HeroStat : CharacterStat
{
    protected override void ResetValue()
    {
        base.ResetValue();

        MaxHealth = 500f;
        MoveSpeed = 2f;
        RotationSpeed = 180f;
        Armor = 0f;
        DamageMultiplier = 0f;
    }
}
