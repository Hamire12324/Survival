using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    private class RuntimePool
    {
        public PoolConfig Config;
        public readonly Queue<PoolObj> Inactive = new();
        public readonly HashSet<PoolObj> Active = new();
        public Transform Parent;
    }

    private static PoolManager instance;
    public static PoolManager Instance
    {
        get
        {
            if (instance != null) return instance;
            instance = FindAnyObjectByType<PoolManager>();
            return instance;
        }
    }

    [SerializeField] private List<PoolConfig> poolConfigs = new();
    private readonly Dictionary<PoolObj, RuntimePool> pools = new();

    private void Awake()
    {
        if (instance != null && instance != this) { Destroy(gameObject); return; }
        instance = this;
        foreach (PoolConfig config in poolConfigs)
            CreatePool(config, true);
    }

    public T Spawn<T>(T prefab, Vector3 position, Quaternion rotation, Transform parent = null) where T : PoolObj
    {
        if (prefab == null) return null;
        RuntimePool pool = GetOrCreatePool(prefab);
        PoolObj obj = pool.Inactive.Count > 0 ? pool.Inactive.Dequeue() : Create(pool);
        if (obj == null) return null;
        obj.transform.SetParent(parent != null ? parent : pool.Parent, false);
        obj.transform.SetPositionAndRotation(position, rotation);
        obj.gameObject.SetActive(true);
        pool.Active.Add(obj);
        obj.OnSpawnedFromPool();
        return obj as T;
    }

    public void Despawn(PoolObj obj)
    {
        if (obj == null || obj.IsInPool || !pools.TryGetValue(obj, out RuntimePool pool)) return;
        pool.Active.Remove(obj);
        obj.OnReturnedToPool();
        obj.transform.SetParent(pool.Parent, false);
        obj.gameObject.SetActive(false);
        pool.Inactive.Enqueue(obj);
    }

    private RuntimePool GetOrCreatePool(PoolObj prefab)
    {
        foreach (RuntimePool pool in pools.Values)
            if (pool.Config.Prefab == prefab) return pool;
        return CreatePool(new PoolConfig { Key = prefab.name, Prefab = prefab }, false);
    }

    private RuntimePool CreatePool(PoolConfig config, bool preload)
    {
        if (config == null || config.Prefab == null) return null;
        GameObject parentObject = new(string.IsNullOrWhiteSpace(config.Key) ? config.Prefab.name + "_Pool" : config.Key + "_Pool");
        parentObject.transform.SetParent(config.Parent != null ? config.Parent : transform);
        RuntimePool pool = new() { Config = config, Parent = parentObject.transform };
        if (preload)
            for (int i = 0; i < Mathf.Min(config.PreloadAmount, config.MaxSize); i++)
            {
                PoolObj obj = Create(pool);
                Despawn(obj);
            }
        return pool;
    }

    private PoolObj Create(RuntimePool pool)
    {
        if (!pool.Config.CanExpand && pool.Active.Count + pool.Inactive.Count >= pool.Config.MaxSize) return null;
        PoolObj obj = Instantiate(pool.Config.Prefab, pool.Parent);
        obj.Initialize(this);
        pools[obj] = pool;
        return obj;
    }
}
