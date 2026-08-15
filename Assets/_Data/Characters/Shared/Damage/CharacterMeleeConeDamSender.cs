using System.Collections.Generic;
using UnityEngine;

/// <summary>Reusable instantaneous melee hit that damages each character once inside a forward cone.</summary>
public class CharacterMeleeConeDamSender : CharacterDamSender
{
    [Header("Cone Hitbox")]
    [SerializeField, Min(0.01f)] private float range = 1.3f;
    [SerializeField, Range(1f, 360f)] private float angle = 50f;
    [SerializeField] private LayerMask targetLayers = Physics.AllLayers;

    private Collider[] overlapResults = new Collider[16];
    private readonly HashSet<CharacterDamReceiver> damagedTargets = new();

    public void DealConeDamage()
    {
        if (characterCtrl == null || characterCtrl.CharacterDamReceiver?.IsDead == true)
            return;

        damagedTargets.Clear();
        int hitCount = Physics.OverlapSphereNonAlloc(
            transform.position, range, overlapResults, targetLayers, QueryTriggerInteraction.Collide);

        if (hitCount == overlapResults.Length)
        {
            System.Array.Resize(ref overlapResults, overlapResults.Length * 2);
            hitCount = Physics.OverlapSphereNonAlloc(
                transform.position, range, overlapResults, targetLayers, QueryTriggerInteraction.Collide);
        }

        float halfAngle = angle * 0.5f;
        for (int index = 0; index < hitCount; index++)
        {
            Collider hit = overlapResults[index];
            overlapResults[index] = null;
            if (hit == null)
                continue;

            CharacterDamReceiver receiver = hit.GetComponentInParent<CharacterDamReceiver>();
            if (receiver == null || !damagedTargets.Add(receiver))
                continue;

            Vector3 toTarget = Vector3.ProjectOnPlane(receiver.transform.position - transform.position, Vector3.up);
            if (toTarget.sqrMagnitude < 0.0001f || Vector3.Angle(transform.forward, toTarget) <= halfAngle)
            {
                if (!TryDealDamage(hit))
                    damagedTargets.Remove(receiver);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 0.55f, 0.05f, 0.9f);
        Gizmos.DrawWireSphere(transform.position, range);
        Quaternion left = Quaternion.AngleAxis(-angle * 0.5f, Vector3.up);
        Quaternion right = Quaternion.AngleAxis(angle * 0.5f, Vector3.up);
        Gizmos.DrawLine(transform.position, transform.position + left * transform.forward * range);
        Gizmos.DrawLine(transform.position, transform.position + right * transform.forward * range);
    }
}
