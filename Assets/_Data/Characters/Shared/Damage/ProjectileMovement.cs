using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    private const int MaxRaycastResults = 8;

    [SerializeField, Min(0.01f)] private float speed = 20f;
    [SerializeField, Min(0.01f)] private float lifetime = 3f;
    [SerializeField] private LayerMask collisionLayer = ~0;
    [SerializeField] private ProjectileDamSender damageSender;

    private readonly RaycastHit[] raycastResults = new RaycastHit[MaxRaycastResults];
    private float remainingLifetime;

    private void Awake()
    {
        damageSender ??= GetComponent<ProjectileDamSender>();
    }

    private void OnEnable()
    {
        remainingLifetime = lifetime;
    }

    private void Update()
    {
        float travelDistance = speed * Time.deltaTime;
        if (TryGetImpact(travelDistance, out RaycastHit impact))
        {
            damageSender.TryHit(impact.collider);
            ReturnProjectileToPool();
            return;
        }

        transform.position += transform.forward * travelDistance;

        remainingLifetime -= Time.deltaTime;
        if (remainingLifetime <= 0f)
            ReturnProjectileToPool();
    }

    private void ReturnProjectileToPool()
    {
        PoolObj poolObj = GetComponent<PoolObj>();
        if (poolObj != null) poolObj.ReturnToPool();
        else gameObject.SetActive(false);
    }

    private bool TryGetImpact(float distance, out RaycastHit closestImpact)
    {
        closestImpact = default;
        int count = Physics.RaycastNonAlloc(
            transform.position,
            transform.forward,
            raycastResults,
            distance,
            collisionLayer,
            QueryTriggerInteraction.Collide);

        float closestDistance = float.PositiveInfinity;
        for (int i = 0; i < count; i++)
        {
            RaycastHit impact = raycastResults[i];
            raycastResults[i] = default;

            if (damageSender.BelongsToOwner(impact.collider) ||
                impact.distance >= closestDistance)
                continue;

            closestImpact = impact;
            closestDistance = impact.distance;
        }

        return closestDistance < float.PositiveInfinity;
    }
}
