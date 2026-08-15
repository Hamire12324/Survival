using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class EnemyWaveSpawner : MonoBehaviour
{
    [System.Serializable]
    private class WaveSpawnEntry
    {
        [SerializeField] private PoolObj enemyPrefab;
        [SerializeField, Min(0)] private int minimumCount = 1;
        [SerializeField, Min(0)] private int maximumCount = 1;

        public PoolObj EnemyPrefab => enemyPrefab;

        public int GetSpawnCount()
        {
            int minimum = Mathf.Max(0, minimumCount);
            int maximum = Mathf.Max(minimum, maximumCount);
            return Random.Range(minimum, maximum + 1);
        }
    }

    [SerializeField] private HeroLevel heroLevel;
    [SerializeField] private Transform[] spawnPoints;
    [Header("Wave Composition")]
    [SerializeField] private List<WaveSpawnEntry> waveEntries = new();
    [SerializeField, Min(0f)] private float nextWaveDelay = 1f;
    [SerializeField, Min(0f)] private float enemyDespawnDelay = 1.5f;

    private readonly List<PoolObj> aliveEnemies = new();
    private float nextWaveTime;
    private int wave;

    private void Start()
    {
        heroLevel ??= FindAnyObjectByType<HeroLevel>();
        StartNextWave();
    }

    private void Update()
    {
        aliveEnemies.RemoveAll(enemy => enemy == null || enemy.IsInPool);
        if (aliveEnemies.Count == 0 && Time.time >= nextWaveTime)
            StartNextWave();
    }

    private void StartNextWave()
    {
        if (spawnPoints == null || spawnPoints.Length == 0 || waveEntries.Count == 0)
            return;

        wave++;
        foreach (WaveSpawnEntry entry in waveEntries)
        {
            if (entry?.EnemyPrefab != null)
                SpawnMany(entry.EnemyPrefab, entry.GetSpawnCount());
        }
        nextWaveTime = float.PositiveInfinity;
    }

    private void SpawnMany(PoolObj prefab, int count)
    {
        PoolManager poolManager = PoolManager.Instance;
        if (poolManager == null)
            return;

        for (int i = 0; i < count; i++)
        {
            Transform point = spawnPoints[Random.Range(0, spawnPoints.Length)];
            PoolObj enemy = poolManager.Spawn(prefab, point.position, point.rotation);
            if (enemy == null) continue;
            aliveEnemies.Add(enemy);
            EnemyExperienceReward reward = enemy.GetComponent<EnemyExperienceReward>();
            if (reward == null)
                reward = enemy.gameObject.AddComponent<EnemyExperienceReward>();
            reward.SetRecipient(heroLevel);
            CharacterDamReceiver receiver = enemy.GetComponentInChildren<CharacterDamReceiver>(true);
            if (receiver == null) continue;
            receiver.OnDeath -= HandleEnemyDeath;
            receiver.OnDeath += HandleEnemyDeath;
        }
    }

    private void HandleEnemyDeath(CharacterDamReceiver receiver)
    {
        receiver.OnDeath -= HandleEnemyDeath;
        PoolObj enemy = receiver.GetComponentInParent<PoolObj>();
        if (enemy != null)
            StartCoroutine(DespawnEnemyAfterDelay(enemy));
    }

    private IEnumerator DespawnEnemyAfterDelay(PoolObj enemy)
    {
        EnemyCtrl enemyCtrl = enemy.GetComponent<EnemyCtrl>();
        if (enemyCtrl?.NavMeshAgent != null && enemyCtrl.NavMeshAgent.isOnNavMesh)
        {
            enemyCtrl.NavMeshAgent.isStopped = true;
            enemyCtrl.NavMeshAgent.ResetPath();
        }

        if (enemyDespawnDelay > 0f)
            yield return new WaitForSeconds(enemyDespawnDelay);

        if (enemy != null && !enemy.IsInPool)
            enemy.ReturnToPool();

        aliveEnemies.Remove(enemy);
        if (aliveEnemies.Count == 0)
            nextWaveTime = Time.time + nextWaveDelay;
    }
}
