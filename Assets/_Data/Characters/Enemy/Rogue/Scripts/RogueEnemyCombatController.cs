using UnityEngine;

public class RogueEnemyCombatController : CharacterCombatController
{
    public override bool TryBasicAttack()
    {
        if (characterCtrl is EnemyCtrl enemyCtrl && enemyCtrl.IsActionLocked)
            return false;

        if (characterCtrl?.CharacterSkillController is not RogueEnemySkillController skillController)
            return false;

        if (!skillController.TryUseBasicAttack())
            return false;

        characterCtrl.CharacterAnimation?.PlayAttackAnimation();
        return true;
    }
}
