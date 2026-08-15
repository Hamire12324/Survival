using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : CharacterMovement
{
    [SerializeField] private EnemyCtrl enemyCtrl;
    [SerializeField, Min(0f)] private float attackRange = 1.3f;

    private EnemyMeleeCombatController combatController;

    protected override void Update()
    {
        if (enemyCtrl?.CharacterDamReceiver?.IsDead == true)
        {
            if (enemyCtrl.NavMeshAgent != null && enemyCtrl.NavMeshAgent.isOnNavMesh)
                enemyCtrl.NavMeshAgent.isStopped = true;
            return;
        }

        CharacterCtrl target = enemyCtrl?.CharacterTargetFinder?.CurrentTarget;
        if (enemyCtrl?.CharacterTargetFinder == null)
            return;

        if (target == null)
            return;

        if (enemyCtrl.NavMeshAgent == null)
            return;

        NavMeshAgent agent = enemyCtrl.NavMeshAgent;
        if (!agent.isOnNavMesh)
            return;

        if (enemyCtrl.IsActionLocked)
        {
            agent.isStopped = true;
            return;
        }

        agent.speed = characterCtrl.CharacterStat != null ? characterCtrl.CharacterStat.MoveSpeed : agent.speed;
        agent.stoppingDistance = attackRange;

        float distance = Vector3.ProjectOnPlane(target.transform.position - transform.position, Vector3.up).magnitude;
        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(target.transform.position);
            return;
        }

        agent.isStopped = true;
        FaceTarget(target.transform.position);
        combatController?.TryBasicAttack(target);
    }

    public bool MoveTowards(Vector3 destination, float stoppingDistance = 0f)
    {
        if (enemyCtrl == null || enemyCtrl.NavMeshAgent == null || !enemyCtrl.NavMeshAgent.isOnNavMesh)
            return false;

        enemyCtrl.NavMeshAgent.stoppingDistance = Mathf.Max(0f, stoppingDistance);
        enemyCtrl.NavMeshAgent.isStopped = false;
        return enemyCtrl.NavMeshAgent.SetDestination(destination);
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        enemyCtrl ??= characterCtrl as EnemyCtrl;
        combatController ??= characterCtrl?.CharacterCombatController as EnemyMeleeCombatController;
        combatController ??= characterCtrl?.GetComponentInChildren<EnemyMeleeCombatController>(true);
    }

    private void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = Vector3.ProjectOnPlane(targetPosition - transform.position, Vector3.up);
        if (direction.sqrMagnitude < 0.0001f)
            return;

        UpdateLookDirection(direction.normalized);
        RotateTowardsLookDirection();
    }

}
