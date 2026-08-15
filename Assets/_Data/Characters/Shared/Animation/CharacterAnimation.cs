using UnityEngine;

public class CharacterAnimation : CharacterAbstract
{
    private const string AttackTrigger = "Basic_Attack";
    private const string HurtTrigger = "Hurt";
    private const string DeathTrigger = "Death";

    protected Vector2 lastMove;

    public virtual void PlayAttackAnimation()
    {
        SetTriggerIfExists(characterCtrl != null ? characterCtrl.Animator : null, AttackTrigger);
    }

    public virtual void PlayHurt()
    {
        SetTriggerIfExists(characterCtrl != null ? characterCtrl.Animator : null, HurtTrigger);
    }

    public virtual void PlayDeath()
    {
        SetTriggerIfExists(characterCtrl != null ? characterCtrl.Animator : null, DeathTrigger);
    }

    public virtual void ResetAfterRevive()
    {
        Animator animator = characterCtrl != null ? characterCtrl.Animator : null;
        if (animator == null) return;

        ResetTriggerIfExists(animator, AttackTrigger);
        ResetTriggerIfExists(animator, HurtTrigger);
        ResetTriggerIfExists(animator, DeathTrigger);

        animator.Rebind();
        if (animator.isActiveAndEnabled && animator.gameObject.activeInHierarchy)
            animator.Update(0f);
    }

    private static void ResetTriggerIfExists(Animator animator, string triggerName)
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.type != AnimatorControllerParameterType.Trigger) continue;
            if (parameter.name != triggerName) continue;

            animator.ResetTrigger(triggerName);
            return;
        }
    }

    private static void SetTriggerIfExists(Animator animator, string triggerName)
    {
        if (animator == null)
            return;

        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.type != AnimatorControllerParameterType.Trigger || parameter.name != triggerName)
                continue;

            animator.SetTrigger(triggerName);
            return;
        }
    }
}
