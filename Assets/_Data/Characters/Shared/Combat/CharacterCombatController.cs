using UnityEngine;
public class CharacterCombatController : CharacterAbstract
{
    [SerializeField, Min(0.01f)] protected float attackCooldown = 0.4f;
    private float nextAttackTime;

    public float AttackCooldown => attackCooldown;

    public virtual bool TryBasicAttack()
    {
        if (!CanStartBasicAttack()) return false;

        nextAttackTime = Time.time + attackCooldown;
        characterCtrl?.CharacterAnimation?.PlayAttackAnimation();
        return true;
    }

    public float GetAttackCooldownRemaining() => Mathf.Max(0f, nextAttackTime - Time.time);

    protected bool CanStartBasicAttack() => characterCtrl != null && Time.time >= nextAttackTime;

    protected void StartBasicAttackCooldown(float cooldown)
    {
        nextAttackTime = Time.time + Mathf.Max(0f, cooldown);
    }

}
