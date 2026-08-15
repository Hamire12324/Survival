using UnityEngine;

public class EnemyPoolObj : PoolObj
{
    [SerializeField] private EnemyCtrl enemyCtrl;
    private void Awake() => enemyCtrl ??= GetComponent<EnemyCtrl>();
    public override void OnSpawnedFromPool()
    {
        base.OnSpawnedFromPool();
        enemyCtrl?.CharacterDamReceiver?.Revive();
        enemyCtrl?.ResetActionLock();

        if (enemyCtrl?.NavMeshAgent != null && enemyCtrl.NavMeshAgent.isOnNavMesh)
        {
            enemyCtrl.NavMeshAgent.ResetPath();
            enemyCtrl.NavMeshAgent.velocity = Vector3.zero;
            enemyCtrl.NavMeshAgent.isStopped = false;
        }
    }
}
