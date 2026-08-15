using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyCtrl : CharacterCtrl
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    public NavMeshAgent NavMeshAgent => navMeshAgent;

    private float actionLockEndTime;
    public bool IsActionLocked => Time.time < actionLockEndTime;

    public void LockActions(float duration)
    {
        actionLockEndTime = Mathf.Max(actionLockEndTime, Time.time + Mathf.Max(0f, duration));
    }

    public void ResetActionLock() => actionLockEndTime = 0f;

    protected override void ResetValue()
    {
        base.ResetValue();

        this.faction = Faction.Enemy;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadNavMeshAgent();
    }
    protected virtual void LoadNavMeshAgent()
    {
        if (navMeshAgent != null) return;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnDrawGizmosSelected()
    {
        EnemyMeleeDamSender meleeDamageSender =
            CharacterDamSender as EnemyMeleeDamSender ??
            GetComponentInChildren<EnemyMeleeDamSender>(true);

        meleeDamageSender?.DrawDamageAreaGizmo();
    }
}
