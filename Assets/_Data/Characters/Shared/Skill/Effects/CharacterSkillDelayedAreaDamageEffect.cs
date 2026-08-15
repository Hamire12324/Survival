using UnityEngine;

[CreateAssetMenu(fileName = "DelayedAreaDamageEffect", menuName = "Survival/Skills/Effects/Delayed Area Damage")]
public sealed class CharacterSkillDelayedAreaDamageEffect : CharacterSkillEffectDefinition
{
    [SerializeField, Min(0f)] private float delay = 2f;
    [SerializeField] private GameObject visualPrefab;
    [SerializeField] private GameObject armVfxPrefab;
    [SerializeField] private CharacterSkillEffectDefinition[] onExplodeEffects;

    public override void Execute(CharacterSkillExecutionContext context)
    {
        if (context.Controller == null || context.Caster == null)
            return;

        GameObject bomb = CreateBomb(context.Caster.transform.position);
        CharacterSkillVfxSpawner.Spawn(armVfxPrefab, bomb.transform.position, Quaternion.identity);
        AudioService.PlayBombPlaced(bomb.transform.position);

        CharacterSkillBomb bombController = bomb.GetComponent<CharacterSkillBomb>();
        bombController.Arm(context, delay, onExplodeEffects);
    }

    private GameObject CreateBomb(Vector3 position)
    {
        if (visualPrefab != null)
        {
            PoolObj poolPrefab = visualPrefab.GetComponent<PoolObj>();
            PoolManager poolManager = PoolManager.Instance;
            if (poolPrefab != null && poolManager != null)
            {
                PoolObj pooledBomb = poolManager.Spawn(poolPrefab, position, Quaternion.identity);
                if (pooledBomb != null)
                    return pooledBomb.gameObject;
            }

            try
            {
                Object instance = Instantiate((Object)visualPrefab, position, Quaternion.identity);
                if (instance is GameObject bomb)
                    return bomb;

                if (instance is Component component)
                    return component.gameObject;

                if (instance != null)
                    Destroy(instance);
            }
            catch (System.InvalidCastException)
            {
                Debug.LogWarning($"Skill bomb visual prefab '{visualPrefab.name}' is not a valid GameObject prefab. " +
                                 "Assign the Bomb1 prefab again in HeroBombPlacement.", visualPrefab);
            }
        }

        return CharacterSkillFeedbackPlayer.CreateBombVfx(position);
    }
}
