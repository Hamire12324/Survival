using UnityEngine;

public class EnemySkillController : CharacterSkillController
{
    [SerializeField] private CharacterSkillDefinition[] skillDefinitions = new CharacterSkillDefinition[4];
    [SerializeField, Min(0f)] private float attackActionLockDuration = 1f;

    private const int BasicAttackSkillIndex = 0;
    private CharacterSkillDefinition activeBasicAttackDefinition;

    public bool TryUseBasicAttack()
    {
        CharacterSkillDefinition basicAttackDefinition = GetSkillDefinition(BasicAttackSkillIndex);
        if (!TryStartDefinitionSkill(BasicAttackSkillIndex, basicAttackDefinition))
            return false;

        activeBasicAttackDefinition = basicAttackDefinition;
        characterCtrl.CharacterAnimation?.PlayAttackAnimation();
        return true;
    }

    public void EnableBasicAttackHitbox()
    {
        if (characterCtrl?.CharacterDamSender is MeleeDamSender meleeDamageSender)
            meleeDamageSender.EnableHitbox();
    }

    public void ApplyBasicAttackImpact()
    {
        ExecuteDefinitionSkill(activeBasicAttackDefinition);
        (characterCtrl as EnemyCtrl)?.LockActions(attackActionLockDuration);
    }

    public void DisableBasicAttackHitbox()
    {
        if (characterCtrl?.CharacterDamSender is MeleeDamSender meleeDamageSender)
            meleeDamageSender.DisableHitbox();
    }

    public override bool TryUseSkill(int index)
    {
        if (index == BasicAttackSkillIndex)
            return TryUseBasicAttack();

        if (index < 0 || index >= skillDefinitions.Length)
            return false;

        return TryUseDefinitionSkill(index, skillDefinitions[index]);
    }

    private CharacterSkillDefinition GetSkillDefinition(int index)
    {
        return index >= 0 && index < skillDefinitions.Length
            ? skillDefinitions[index]
            : null;
    }
}
