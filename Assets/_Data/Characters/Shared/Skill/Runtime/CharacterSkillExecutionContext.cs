using UnityEngine;

public readonly struct CharacterSkillExecutionContext
{
    public CharacterSkillController Controller { get; }
    public CharacterCtrl Caster { get; }
    public CharacterSkillDefinition Definition { get; }

    public CharacterSkillExecutionContext(
        CharacterSkillController controller,
        CharacterSkillDefinition definition)
    {
        Controller = controller;
        Caster = controller != null ? controller.CharacterCtrl : null;
        Definition = definition;
    }
}
