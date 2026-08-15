using UnityEngine;

public class HeroDamReceiver : CharacterDamReceiver
{
    public override void ReceiveDamage(float damage, Transform attacker = null, DamageData damageData = null)
    {
        if (IsDead || IsInvincible || CharacterCtrl?.CharacterStat == null)
            return;

        float finalDamage = CharacterCtrl.CharacterStat.CalculateReceivedDamage(damage);
        base.ReceiveDamage(damage, attacker, damageData);
        if (finalDamage > 0f)
            CameraShake.ShakePlayerHit();
    }

    protected override void Die(Transform killer = null)
    {
        base.Die(killer);
        (CharacterCtrl as HeroCtrl)?.SetInputEnabled(false);
    }

    public override void Revive()
    {
        base.Revive();
        (CharacterCtrl as HeroCtrl)?.SetInputEnabled(true);
    }
}
