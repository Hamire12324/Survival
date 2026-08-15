using UnityEngine;

[CreateAssetMenu(fileName = "AreaDamageEffect", menuName = "Survival/Skills/Effects/Area Damage")]
public sealed class CharacterSkillAreaDamageEffect : CharacterSkillEffectDefinition
{
    [SerializeField, Min(0.01f)] private float radius = 3f;
    [SerializeField] private DamageData damageData = new(15f);
    [SerializeField] private LayerMask targetLayers = Physics.AllLayers;
    [SerializeField] private QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Collide;

    public override void Execute(CharacterSkillExecutionContext context)
    {
        if (context.Caster == null)
            return;

        CharacterSkillAreaDamageUtility.DealDamage(
            context.Caster,
            context.Caster.transform.position,
            radius,
            damageData,
            targetLayers,
            triggerInteraction);
    }
}
