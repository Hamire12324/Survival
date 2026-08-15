using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class MeleeDamSender : CharacterDamSender
{
    private const int MaxOverlapResults = 20;

    [Header("Melee Hitbox")]
    [SerializeField] private BoxCollider hitboxCollider;
    [SerializeField] protected LayerMask targetLayer;

    private readonly Collider[] overlapResults = new Collider[MaxOverlapResults];
    private readonly HashSet<CharacterDamReceiver> damagedTargets = new();

    protected override void LoadComponents()
    {
        base.LoadComponents();
        hitboxCollider ??= GetComponent<BoxCollider>();
    }

    protected override void OnDisable()
    {
        DisableHitbox();
        damagedTargets.Clear();
        base.OnDisable();
    }

    public void DealHitboxDamage(DamageData damageData = null)
    {
        if (hitboxCollider == null || !hitboxCollider.enabled)
            return;

        int count = Physics.OverlapBoxNonAlloc(
            hitboxCollider.transform.TransformPoint(hitboxCollider.center),
            Vector3.Scale(hitboxCollider.size * 0.5f, GetAbsoluteLossyScale()),
            overlapResults,
            hitboxCollider.transform.rotation,
            targetLayer,
            QueryTriggerInteraction.Collide);

        for (int i = 0; i < count; i++)
        {
            Collider hitCollider = overlapResults[i];
            overlapResults[i] = null;

            CharacterCtrl targetCtrl = hitCollider.GetComponentInParent<CharacterCtrl>();
            CharacterDamReceiver receiver = targetCtrl?.CharacterDamReceiver ??
                                           hitCollider.GetComponentInParent<CharacterDamReceiver>();

            if (receiver == null)
            {
                continue;
            }

            if (!damagedTargets.Add(receiver))
            {
                continue;
            }

            if (!IsWithinDamageArea(hitCollider))
            {
                damagedTargets.Remove(receiver);
                continue;
            }

            if (!TryDealDamage(hitCollider, damageData))
            {
                damagedTargets.Remove(receiver);
            }
        }
    }

    public void EnableHitbox()
    {
        damagedTargets.Clear();
        if (hitboxCollider != null)
            hitboxCollider.enabled = true;
    }

    public void DisableHitbox()
    {
        if (hitboxCollider != null)
            hitboxCollider.enabled = false;

        damagedTargets.Clear();
    }

    private Vector3 GetAbsoluteLossyScale()
    {
        Vector3 scale = hitboxCollider.transform.lossyScale;
        return new Vector3(Mathf.Abs(scale.x), Mathf.Abs(scale.y), Mathf.Abs(scale.z));
    }

    protected virtual bool IsWithinDamageArea(Collider hitCollider) => true;
}
