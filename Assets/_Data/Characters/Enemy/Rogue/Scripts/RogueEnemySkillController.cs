using UnityEngine;

public class RogueEnemySkillController : CharacterSkillController
{
    [SerializeField] private CharacterSkillDefinition[] skillDefinitions = new CharacterSkillDefinition[4];

    public bool TryUseBasicAttack() => TryUseDefinitionSkill(0, GetSkillDefinition(0));

    public override bool TryUseSkill(int index)
    {
        return TryUseDefinitionSkill(index, GetSkillDefinition(index));
    }

    private CharacterSkillDefinition GetSkillDefinition(int index)
    {
        return index >= 0 && index < skillDefinitions.Length
            ? skillDefinitions[index]
            : null;
    }
}
