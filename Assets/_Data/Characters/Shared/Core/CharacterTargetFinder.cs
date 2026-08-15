using UnityEngine;

[DisallowMultipleComponent]
public class CharacterTargetFinder : CharacterAbstract
{
    [SerializeField, Min(0.1f)] private float searchRadius = 12f;
    [SerializeField, Min(0.02f)] private float refreshInterval = 0.2f;
    [SerializeField] protected LayerMask targetLayers = Physics.AllLayers;

    private Collider[] overlapResults = new Collider[32];
    private float nextRefreshTime;

    public CharacterCtrl CurrentTarget { get; private set; }

    protected override void Update()
    {
        if (Time.time >= nextRefreshTime)
            FindTarget();
    }

    public CharacterCtrl FindTarget()
    {
        nextRefreshTime = Time.time + refreshInterval;
        if (characterCtrl == null || characterCtrl.CharacterDamReceiver?.IsDead == true)
        {
            return CurrentTarget = null;
        }

        int hitCount = Physics.OverlapSphereNonAlloc(
            transform.position, searchRadius, overlapResults, targetLayers, QueryTriggerInteraction.Collide);

        if (hitCount == overlapResults.Length)
        {
            System.Array.Resize(ref overlapResults, overlapResults.Length * 2);
            hitCount = Physics.OverlapSphereNonAlloc(
                transform.position, searchRadius, overlapResults, targetLayers, QueryTriggerInteraction.Collide);
        }

        CharacterCtrl closest = null;
        float closestSqrDistance = float.MaxValue;
        for (int index = 0; index < hitCount; index++)
        {
            Collider hit = overlapResults[index];
            overlapResults[index] = null;
            CharacterCtrl candidate = hit != null ? hit.GetComponentInParent<CharacterCtrl>() : null;
            if (candidate == null || candidate == characterCtrl ||
                candidate.CharacterDamReceiver == null || candidate.CharacterDamReceiver.IsDead ||
                !FactionManager.CanAttack(characterCtrl.Faction, candidate.Faction))
                continue;

            float sqrDistance = Vector3.ProjectOnPlane(
                candidate.transform.position - transform.position, Vector3.up).sqrMagnitude;
            if (sqrDistance < closestSqrDistance)
            {
                closestSqrDistance = sqrDistance;
                closest = candidate;
            }
        }

        CurrentTarget = closest;
        return CurrentTarget;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.85f, 0.1f, 0.75f);
        Gizmos.DrawWireSphere(transform.position, searchRadius);
    }
}
