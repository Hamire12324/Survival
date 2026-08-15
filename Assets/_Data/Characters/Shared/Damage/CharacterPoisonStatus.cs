using System.Collections;
using UnityEngine;

public class CharacterPoisonStatus : MonoBehaviour
{
    private Coroutine poisonRoutine;

    public void Apply(CharacterDamReceiver receiver, CharacterCtrl attacker, DamageData damageData)
    {
        if (receiver == null || receiver.IsDead || damageData == null)
            return;

        if (poisonRoutine != null)
            StopCoroutine(poisonRoutine);

        DealTick(receiver, attacker, damageData);
        poisonRoutine = StartCoroutine(PoisonRoutine(receiver, attacker, damageData));
    }

    private IEnumerator PoisonRoutine(CharacterDamReceiver receiver, CharacterCtrl attacker, DamageData damageData)
    {
        for (int tick = 0; tick < 3; tick++)
        {
            yield return new WaitForSeconds(1f);
            if (receiver == null || receiver.IsDead)
                break;

            DealTick(receiver, attacker, damageData);
        }

        poisonRoutine = null;
    }

    private static void DealTick(CharacterDamReceiver receiver, CharacterCtrl attacker, DamageData damageData)
    {
        float damage = attacker?.CharacterStat != null
            ? attacker.CharacterStat.CalculateDealtDamage(damageData.BaseDamage)
            : damageData.BaseDamage;
        receiver.ReceiveDamage(damage, attacker != null ? attacker.transform : null, damageData);
    }
}
