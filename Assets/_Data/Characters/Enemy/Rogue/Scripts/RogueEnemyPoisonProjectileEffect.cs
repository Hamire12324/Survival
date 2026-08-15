using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "PoisonProjectileEffect", menuName = "Survival/Skills/Effects/Poison Projectile")]
public class RogueEnemyPoisonProjectileEffect : CharacterSkillEffectDefinition
{
    [SerializeField] private PoolObj projectilePrefab;
    [SerializeField] private DamageData poisonDamage = new(30f);
    [SerializeField] private Vector3 spawnOffset = new(0f, 1f, 0.6f);
    [Header("Cast Timing")]
    [SerializeField, Min(0f)] private float spawnDelay = 0.25f;
    [SerializeField, Min(0f)] private float actionLockDuration = 1f;

    public override void Execute(CharacterSkillExecutionContext context)
    {
        if (context.Caster == null || projectilePrefab == null)
            return;

        if (spawnDelay > 0f && context.Controller != null)
        {
            context.Controller.StartCoroutine(SpawnAfterDelay(context));
            return;
        }

        SpawnProjectile(context);
    }

    private IEnumerator SpawnAfterDelay(CharacterSkillExecutionContext context)
    {
        yield return new WaitForSeconds(spawnDelay);
        if (context.Caster?.CharacterDamReceiver?.IsDead == true)
            yield break;

        SpawnProjectile(context);
    }

    private void SpawnProjectile(CharacterSkillExecutionContext context)
    {
        if (context.Caster == null || projectilePrefab == null)
            return;

        PoolManager poolManager = PoolManager.Instance;
        if (poolManager == null)
            return;

        PoolObj pooledProjectile = poolManager.Spawn(
            projectilePrefab,
            context.Caster.transform.TransformPoint(spawnOffset),
            context.Caster.transform.rotation);
        RogueEnemyPoisonProjectile projectile = pooledProjectile != null
            ? pooledProjectile.GetComponent<RogueEnemyPoisonProjectile>()
            : null;
        if (projectile == null)
        {
            pooledProjectile?.ReturnToPool();
            return;
        }
        projectile.ConfigurePoison(context.Caster, poisonDamage);
        (context.Caster as EnemyCtrl)?.LockActions(actionLockDuration);
    }
}
