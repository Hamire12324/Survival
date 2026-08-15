using UnityEngine;

[System.Serializable]
public class DamageData
{
    [Min(0f)] public float BaseDamage = 1f;
    public bool CanCrit;

    [Header("Hit Stun")]
    public bool CausesHitStun;
    public float HitStunDuration = 0.2f;
    public float HitStunImmunityDuration = 0.75f;
    public bool IgnoresHitStunImmunity;
    public bool InterruptsAttack = true;

    public DamageData(float baseDamage, bool canCrit = false)
    {
        BaseDamage = Mathf.Max(0f, baseDamage);
        CanCrit = canCrit;
    }

}
