using UnityEngine;

public static class CharacterSkillVfxSpawner
{
    public static GameObject Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        if (prefab == null)
            return null;

        PoolObj poolPrefab = prefab.GetComponent<PoolObj>();
        PoolManager poolManager = PoolManager.Instance;
        if (poolPrefab != null && poolManager != null)
            return poolManager.Spawn(poolPrefab, position, rotation)?.gameObject;

        return Object.Instantiate(prefab, position, rotation);
    }
}
