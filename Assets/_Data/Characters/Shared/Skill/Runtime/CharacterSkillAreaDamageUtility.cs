using System.Collections.Generic;
using UnityEngine;

public static class CharacterSkillAreaDamageUtility
{
    private static Collider[] overlapResults = new Collider[32];

    public static int DealDamage(
        CharacterCtrl caster,
        Vector3 center,
        float radius,
        DamageData damageData,
        LayerMask targetLayers,
        QueryTriggerInteraction triggerInteraction)
    {
        if (caster == null || caster.CharacterStat == null || damageData == null)
            return 0;

        HashSet<CharacterCtrl> targets = new();
        int hitCount = OverlapSphere(center, Mathf.Max(0f, radius), targetLayers, triggerInteraction);
        for (int index = 0; index < hitCount; index++)
        {
            Collider hit = overlapResults[index];
            if (hit == null)
                continue;

            CharacterCtrl target = hit.GetComponentInParent<CharacterCtrl>();
            if (target != null)
                targets.Add(target);
        }

        int damagedTargetCount = 0;
        float dealtDamage = caster.CharacterStat.CalculateDealtDamage(damageData.BaseDamage);
        foreach (CharacterCtrl target in targets)
        {
            if (target == caster || target.CharacterDamReceiver == null || target.CharacterDamReceiver.IsDead ||
                !FactionManager.CanAttack(caster.Faction, target.Faction))
                continue;

            target.CharacterDamReceiver.ReceiveDamage(dealtDamage, caster.transform, damageData);
            damagedTargetCount++;
        }

        return damagedTargetCount;
    }

    private static int OverlapSphere(
        Vector3 center,
        float radius,
        LayerMask targetLayers,
        QueryTriggerInteraction triggerInteraction)
    {
        while (true)
        {
            int hitCount = Physics.OverlapSphereNonAlloc(
                center,
                radius,
                overlapResults,
                targetLayers,
                triggerInteraction);

            if (hitCount < overlapResults.Length || overlapResults.Length >= 1024)
                return hitCount;

            System.Array.Resize(ref overlapResults, overlapResults.Length * 2);
        }
    }
}
