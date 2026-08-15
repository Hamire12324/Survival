using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDamSender : CharacterAbstract
{
    [Header("Damage Settings")]
    [SerializeField] protected DamageData hitDamage;

    private readonly Dictionary<CharacterDamReceiver, Coroutine> dotCoroutines = new();

    protected override void OnDisable()
    {
        StopAllCoroutines();
        dotCoroutines.Clear();
        base.OnDisable();
    }

    protected bool TryDealDamage(Collider hitCollider, DamageData damageData = null)
    {
        CharacterCtrl targetCtrl = hitCollider.GetComponentInParent<CharacterCtrl>();
        if (!IsValidTarget(hitCollider, targetCtrl))
            return false;

        DealDamage(targetCtrl.CharacterDamReceiver, damageData ?? hitDamage);
        return true;
    }

    protected virtual bool IsValidTarget(Collider hitCollider, CharacterCtrl targetCtrl)
    {
        if (targetCtrl == null || targetCtrl == characterCtrl)
            return false;

        if (targetCtrl.CharacterDamReceiver == null || targetCtrl.CharacterDamReceiver.IsDead)
            return false;

        if (hitCollider != targetCtrl.CharacterCollider)
            return false;

        return FactionManager.CanAttack(characterCtrl.Faction, targetCtrl.Faction);
    }

    public virtual void DealDamage(CharacterDamReceiver target)
    {
        DealDamage(target, hitDamage);
    }

    public virtual void DealDamage(CharacterDamReceiver target, DamageData damageData)
    {
        if (target == null || target.IsDead)
            return;

        float damage = CalculateDamage(damageData);
        target.ReceiveDamage(damage, transform, damageData);
    }

    public virtual void DealDamageOverTime(
        CharacterDamReceiver target,
        float totalDamage,
        float duration,
        int ticks = 5)
    {
        if (target == null || target.IsDead)
            return;

        ticks = Mathf.Max(1, ticks);
        duration = Mathf.Max(0f, duration);

        if (dotCoroutines.TryGetValue(target, out Coroutine existing))
            StopCoroutine(existing);

        dotCoroutines[target] = StartCoroutine(
            DamageOverTimeCoroutine(target, totalDamage, duration, ticks));
    }

    protected virtual float CalculateDamage()
    {
        return CalculateDamage(hitDamage);
    }

    protected virtual float CalculateDamage(DamageData damageData)
    {
        CharacterCtrl owner = characterCtrl ?? GetComponentInParent<CharacterCtrl>();
        CharacterStat ownerStat = owner?.CharacterStat ??
                                  owner?.GetComponentInChildren<CharacterStat>(true);

        if (ownerStat == null)
            return 0f;

        characterCtrl = owner;
        return ownerStat.CalculateDealtDamage(
            damageData?.BaseDamage ?? 1f);
    }

    public virtual void SetDamageData(DamageData data) => hitDamage = data;

    public void Configure(CharacterCtrl owner, DamageData data = null)
    {
        characterCtrl = owner;
        if (data != null)
            hitDamage = data;
    }

    public bool BelongsToOwner(Collider hitCollider) =>
        characterCtrl != null &&
        hitCollider.GetComponentInParent<CharacterCtrl>() == characterCtrl;

    private IEnumerator DamageOverTimeCoroutine(
        CharacterDamReceiver target,
        float totalDamage,
        float duration,
        int ticks)
    {
        float damagePerTick = totalDamage / ticks;
        float interval = duration / ticks;

        for (int i = 0; i < ticks; i++)
        {
            if (target == null || target.IsDead)
                yield break;

            target.ReceiveDamage(damagePerTick, transform, hitDamage);

            if (i < ticks - 1 && interval > 0f)
                yield return new WaitForSeconds(interval);
        }

        dotCoroutines.Remove(target);
    }
}
