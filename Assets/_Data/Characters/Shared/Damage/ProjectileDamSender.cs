using UnityEngine;

public class ProjectileDamSender : CharacterDamSender
{
    [Header("Projectile")]
    [SerializeField] private bool destroyOnHit = true;

    private bool hasHit;

    protected override void LoadCharacterCtrl()
    {
        // Projectiles are pooled independently and receive their owner in Configure().
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        hasHit = false;
    }
    public bool TryHit(Collider hitCollider)
    {
        if (hasHit || !TryDealDamage(hitCollider))
            return false;

        hasHit = true;
        if (destroyOnHit)
            gameObject.SetActive(false);

        return true;
    }
}
