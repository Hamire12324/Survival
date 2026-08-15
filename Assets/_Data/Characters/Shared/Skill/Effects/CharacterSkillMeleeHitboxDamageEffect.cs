using UnityEngine;

[CreateAssetMenu(fileName = "MeleeHitboxDamageEffect", menuName = "Survival/Skills/Effects/Melee Hitbox Damage")]
public class CharacterSkillMeleeHitboxDamageEffect : CharacterSkillEffectDefinition
{
    [SerializeField] private DamageData damageData = new(30f);

    public override void Execute(CharacterSkillExecutionContext context)
    {
        if (context.Caster?.CharacterDamSender is MeleeDamSender meleeDamageSender)
            meleeDamageSender.DealHitboxDamage(damageData);
    }
}
