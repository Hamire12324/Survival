using UnityEngine;
using UnityEngine.AI;

public class RogueEnemyMovement : CharacterMovement
{
    [SerializeField, Min(0.01f)] private float attackRange = 3f;
    private EnemyCtrl enemyCtrl;
    private RogueEnemyCombatController combatController;

    protected override void Update()
    {
        enemyCtrl ??= characterCtrl as EnemyCtrl;
        combatController ??= characterCtrl?.CharacterCombatController as RogueEnemyCombatController;
        if (enemyCtrl?.CharacterDamReceiver?.IsDead == true)
        {
            if (enemyCtrl.NavMeshAgent != null && enemyCtrl.NavMeshAgent.isOnNavMesh)
                enemyCtrl.NavMeshAgent.isStopped = true;
            return;
        }

        CharacterCtrl target = enemyCtrl?.CharacterTargetFinder?.CurrentTarget;
        NavMeshAgent agent = enemyCtrl?.NavMeshAgent;
        if (target == null || agent == null || !agent.isOnNavMesh)
            return;

        if (enemyCtrl.IsActionLocked)
        {
            agent.isStopped = true;
            return;
        }

        float distance = Vector3.ProjectOnPlane(target.transform.position - transform.position, Vector3.up).magnitude;
        agent.speed = characterCtrl.CharacterStat.MoveSpeed;
        agent.stoppingDistance = attackRange;
        if (distance > attackRange)
        {
            agent.isStopped = false;
            agent.SetDestination(target.transform.position);
            return;
        }

        agent.isStopped = true;
        Vector3 direction = Vector3.ProjectOnPlane(target.transform.position - transform.position, Vector3.up);
        if (direction.sqrMagnitude > 0.0001f)
        {
            UpdateLookDirection(direction.normalized);
            RotateTowardsLookDirection();
        }

        if (Vector3.Angle(transform.forward, direction) <= 5f)
            combatController?.TryBasicAttack();
    }
}
