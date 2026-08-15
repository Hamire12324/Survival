using UnityEngine;

/// <summary>Spawns the configured pooled hit effect at damaged characters.</summary>
public sealed class HitVfxService : MonoBehaviour
{
    private static HitVfxService instance;

    [SerializeField] private VFXPoolObj hitVfxPrefab;
    [SerializeField] private Vector3 spawnOffset = new(0f, 1f, 0f);

    private void Awake() => instance = this;

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public static void Play(Vector3 position) => instance?.Spawn(position);

    private void Spawn(Vector3 position)
    {
        if (hitVfxPrefab == null || PoolManager.Instance == null)
            return;

        PoolManager.Instance.Spawn(hitVfxPrefab, position + spawnOffset, Quaternion.identity);
    }
}
