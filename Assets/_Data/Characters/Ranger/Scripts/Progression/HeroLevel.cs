using UnityEngine;

public class HeroLevel : CharacterLevel
{
    [Header("Level-up reward")]
    [SerializeField, Min(0f)] private float maxHealthReward = 40f;
    [SerializeField, Min(0f)] private float armorReward = 2f;
    [SerializeField] private float damageMultiplierReward = 0.1f;

    protected override void ApplyLevelUpReward()
    {
        CharacterCtrl?.CharacterStat?.AddMaxHealth(maxHealthReward);
        CharacterCtrl?.CharacterStat?.AddArmor(armorReward);
        CharacterCtrl?.CharacterStat?.AddDamageMultiplier(damageMultiplierReward);
        AudioService.PlayLevelUp();
    }
}
