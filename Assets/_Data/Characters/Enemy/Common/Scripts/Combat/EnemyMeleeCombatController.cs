using UnityEngine;

public class EnemyMeleeCombatController : CharacterCombatController
{
    [SerializeField, Range(0f, 45f)] private float attackFacingTolerance = 5f;

    private EnemySkillController SkillController =>
        characterCtrl != null ? characterCtrl.CharacterSkillController as EnemySkillController : null;

    public override bool TryBasicAttack()
    {
        if (characterCtrl is EnemyCtrl enemyCtrl && enemyCtrl.IsActionLocked)
            return false;

        if (SkillController == null || !SkillController.TryUseBasicAttack())
            return false;

        return true;
    }

    public bool TryBasicAttack(CharacterCtrl target)
    {
        if (!IsFacingTarget(target))
            return false;

        return TryBasicAttack();
    }

    public bool IsFacingTarget(CharacterCtrl target) => GetFacingAngle(target) <= attackFacingTolerance;

    public float GetFacingAngle(CharacterCtrl target)
    {
        if (target == null || characterCtrl == null)
            return 180f;

        Vector3 direction = Vector3.ProjectOnPlane(
            target.transform.position - characterCtrl.transform.position,
            Vector3.up);
        return direction.sqrMagnitude < 0.0001f
            ? 180f
            : Vector3.Angle(characterCtrl.transform.forward, direction);
    }

    public void EnableAttackHitbox() => SkillController?.EnableBasicAttackHitbox();
    public void ApplyAttackHit() => SkillController?.ApplyBasicAttackImpact();
    public void DisableAttackHitbox() => SkillController?.DisableBasicAttackHitbox();

    protected override void OnDisable()
    {
        DisableAttackHitbox();
        base.OnDisable();
    }
}
