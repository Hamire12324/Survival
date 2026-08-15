using UnityEngine;

public class RogueEnemyPoisonProjectile : ProjectileDamSender
{
    private DamageData poisonDamage;

    // Projectiles are spawned without a CharacterCtrl parent. ConfigurePoison
    // assigns their owner immediately after Instantiate.
    protected override void LoadCharacterCtrl()
    {
    }

    public void ConfigurePoison(CharacterCtrl owner, DamageData damageData)
    {
        Configure(owner, damageData);
        poisonDamage = damageData;
    }

    public override void DealDamage(CharacterDamReceiver target, DamageData damageData)
    {
        if (target == null || target.IsDead)
            return;

        CharacterPoisonStatus poisonStatus = target.GetComponent<CharacterPoisonStatus>();
        if (poisonStatus == null)
            poisonStatus = target.gameObject.AddComponent<CharacterPoisonStatus>();

        poisonStatus.Apply(target, characterCtrl, poisonDamage ?? damageData);
    }
}
