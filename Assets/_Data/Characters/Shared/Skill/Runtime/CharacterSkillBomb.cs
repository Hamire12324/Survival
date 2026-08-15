using System.Collections;
using UnityEngine;

public sealed class CharacterSkillBomb : MonoBehaviour
{
    [SerializeField, Min(0.01f)] private float explosionRadius = 5f;

    private CharacterSkillExecutionContext context;
    private CharacterSkillEffectDefinition[] onExplodeEffects;
    private bool isArmed;

    private void Awake()
    {
        foreach (Collider collider in GetComponentsInChildren<Collider>(true))
            collider.enabled = false;
    }

    public void Arm(
        CharacterSkillExecutionContext executionContext,
        float fuseDuration,
        CharacterSkillEffectDefinition[] effects)
    {
        context = executionContext;
        onExplodeEffects = effects;
        SyncExplosionRadius(effects);
        isArmed = true;
        StopAllCoroutines();
        StartCoroutine(ExplodeAfterDelay(Mathf.Max(0f, fuseDuration)));
    }

    private void SyncExplosionRadius(CharacterSkillEffectDefinition[] effects)
    {
        float largestRadius = 0f;
        foreach (CharacterSkillEffectDefinition effect in effects ?? System.Array.Empty<CharacterSkillEffectDefinition>())
        {
            if (effect != null && effect.TryGetAreaRadius(out float effectRadius))
                largestRadius = Mathf.Max(largestRadius, effectRadius);
        }

        if (largestRadius > 0f)
            explosionRadius = largestRadius;
    }

    private IEnumerator ExplodeAfterDelay(float fuseDuration)
    {
        if (fuseDuration > 0f)
            yield return new WaitForSeconds(fuseDuration);

        foreach (CharacterSkillEffectDefinition effect in onExplodeEffects ?? System.Array.Empty<CharacterSkillEffectDefinition>())
            effect?.ExecuteAtPosition(context, transform.position);

        yield return null;
        PoolObj poolObj = GetComponent<PoolObj>();
        if (poolObj != null) poolObj.ReturnToPool();
        else Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying || !isArmed)
            return;

        Gizmos.color = new Color(1f, 0.15f, 0.05f, 0.9f);
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
