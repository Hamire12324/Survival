using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "BowProjectileEffect", menuName = "Survival/Skills/Effects/Bow Projectile")]
public class HeroBowProjectileEffect : CharacterSkillEffectDefinition
{
    [SerializeField] private DamageData damageData = new(10f);
    [SerializeField] private float[] angleOffsets = { -15f, 0f, 15f };
    [Header("Cast Timing")]
    [SerializeField, Min(0f)] private float spawnDelay = 0.25f;

    public override void Execute(CharacterSkillExecutionContext context)
    {
        if (context.Caster == null)
            return;

        if (spawnDelay > 0f && context.Controller != null)
        {
            context.Controller.StartCoroutine(SpawnAfterDelay(context));
            return;
        }

        SpawnProjectiles(context);
    }

    private IEnumerator SpawnAfterDelay(CharacterSkillExecutionContext context)
    {
        yield return new WaitForSeconds(spawnDelay);
        if (context.Caster?.CharacterDamReceiver?.IsDead == true)
            yield break;

        SpawnProjectiles(context);
    }

    private void SpawnProjectiles(CharacterSkillExecutionContext context)
    {
        HeroBowCombatController combat = context.Caster != null
            ? context.Caster.CharacterCombatController as HeroBowCombatController
            : null;
        combat?.ShootSpread(damageData, angleOffsets);
    }
}
