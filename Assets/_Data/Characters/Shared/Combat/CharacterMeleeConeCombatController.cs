using UnityEngine;

public class CharacterMeleeConeCombatController : CharacterCombatController
{
    [SerializeField] private CharacterMeleeConeDamSender coneDamageSender;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        coneDamageSender ??= GetComponentInParent<CharacterMeleeConeDamSender>();
    }

    public override bool TryBasicAttack()
    {
        if (!CanStartBasicAttack() || coneDamageSender == null ||
            characterCtrl?.CharacterDamReceiver?.IsDead == true)
            return false;

        StartBasicAttackCooldown(attackCooldown);
        coneDamageSender.DealConeDamage();
        return true;
    }
}
