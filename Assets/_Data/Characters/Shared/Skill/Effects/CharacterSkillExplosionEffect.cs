using UnityEngine;

[CreateAssetMenu(fileName = "ExplosionEffect", menuName = "Survival/Skills/Effects/Explosion")]
public sealed class CharacterSkillExplosionEffect : CharacterSkillEffectDefinition
{
    [SerializeField, Min(0.01f)] private float radius = 3f;
    [SerializeField] private DamageData damageData = new(15f);
    [SerializeField] private LayerMask targetLayers = Physics.AllLayers;
    [SerializeField] private QueryTriggerInteraction triggerInteraction = QueryTriggerInteraction.Collide;
    [SerializeField] private GameObject vfxPrefab;

    public override bool TryGetAreaRadius(out float areaRadius)
    {
        areaRadius = radius;
        return true;
    }

    public override void Execute(CharacterSkillExecutionContext context)
    {
        if (context.Caster != null)
            ExecuteAtPosition(context, context.Caster.transform.position);
    }

    public override void ExecuteAtPosition(CharacterSkillExecutionContext context, Vector3 position)
    {
        if (context.Caster == null)
            return;

        // This definition is the authority for gameplay radius. Any bomb collider is
        // synchronized from this value, never the other way around.
        CharacterSkillAreaDamageUtility.DealDamage(
            context.Caster,
            position,
            radius,
            damageData,
            targetLayers,
            triggerInteraction);
        CharacterSkillVfxSpawner.Spawn(vfxPrefab, position, Quaternion.identity);
        AudioService.PlayExplosion(position);
    }
}
