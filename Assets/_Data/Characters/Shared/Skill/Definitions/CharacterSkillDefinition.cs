using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSkill", menuName = "Survival/Skills/Skill Definition")]
public class CharacterSkillDefinition : ScriptableObject
{
    [SerializeField] private string skillId;
    [SerializeField] private string displayName;
    [SerializeField, Min(0f)] private float cooldown;
    [SerializeField] private List<CharacterSkillEffectDefinition> effects = new();

    public string SkillId => skillId;
    public string DisplayName => displayName;
    public float Cooldown => cooldown;
    public IReadOnlyList<CharacterSkillEffectDefinition> Effects => effects;
}
