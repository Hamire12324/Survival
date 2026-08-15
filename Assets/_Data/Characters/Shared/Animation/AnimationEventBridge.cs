using UnityEngine;

public class AnimationEventBridge : CharacterAbstract
{
    public void EnableAttackHitbox()
    {
        if (CharacterCtrl?.CharacterCombatController is EnemyMeleeCombatController combat)
            combat.EnableAttackHitbox();
    }

    public void ApplyAttackHit()
    {
        if (CharacterCtrl?.CharacterCombatController is EnemyMeleeCombatController combat)
            combat.ApplyAttackHit();
    }

    public void DisableAttackHitbox()
    {
        if (CharacterCtrl?.CharacterCombatController is EnemyMeleeCombatController combat)
            combat.DisableAttackHitbox();
    }
}
