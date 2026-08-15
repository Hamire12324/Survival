using UnityEngine;

[DisallowMultipleComponent]
public class PoolObj : MonoBehaviour
{
    private PoolManager owner;
    public bool IsInPool { get; private set; }

    public void Initialize(PoolManager pool) => owner = pool;
    public virtual void OnSpawnedFromPool() => IsInPool = false;
    public virtual void OnReturnedToPool() => IsInPool = true;
    public void ReturnToPool()
    {
        if (owner != null) owner.Despawn(this);
        else gameObject.SetActive(false);
    }
}
