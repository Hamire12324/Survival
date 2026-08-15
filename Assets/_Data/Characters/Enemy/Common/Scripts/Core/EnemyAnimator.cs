using UnityEngine.AI;

public class EnemyAnimator : CharacterAnimation
{
    private EnemyCtrl enemyCtrl;

    protected override void Update()
    {
        enemyCtrl ??= characterCtrl as EnemyCtrl;
        if (characterCtrl?.Animator == null)
            return;

        NavMeshAgent agent = enemyCtrl?.NavMeshAgent;
        float speed = agent != null && agent.isOnNavMesh && !agent.isStopped &&
                      agent.velocity.sqrMagnitude > 0.01f
            ? 1f
            : 0f;
        characterCtrl.Animator.SetFloat("Speed", speed);
    }

    public override void PlayAttackAnimation()
    {
        if (characterCtrl?.Animator != null)
            base.PlayAttackAnimation();
    }

    public override void PlayHurt()
    {
        if (characterCtrl?.Animator != null)
            base.PlayHurt();
    }

    public override void PlayDeath()
    {
        if (characterCtrl?.Animator != null)
            base.PlayDeath();
    }
}
