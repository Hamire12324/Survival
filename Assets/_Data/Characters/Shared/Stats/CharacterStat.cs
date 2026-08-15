using System;
using UnityEngine;
public class CharacterStat : CharacterAbstract
{
    [Header("Core Stats")]
    [SerializeField, Min(0f)] private float maxHealth;
    [SerializeField, Min(0f)] private float moveSpeed;
    [SerializeField, Min(0f)] private float rotationSpeed;
    [SerializeField, Min(0f)] private float armor;
    [SerializeField] private float damageMultiplier;

    [SerializeField, Min(0f)] private float currentHealth;

    public float MaxHealth
    {
        get => maxHealth;
        protected set => maxHealth = Mathf.Max(0f, value);
    }

    public float MoveSpeed
    {
        get => moveSpeed;
        protected set => moveSpeed = Mathf.Max(0f, value);
    }

    public float RotationSpeed
    {
        get => rotationSpeed;
        protected set => rotationSpeed = Mathf.Max(0f, value);
    }

    public float Armor
    {
        get => armor;
        protected set => armor = Mathf.Max(0f, value);
    }

    public float DamageMultiplier
    {
        get => damageMultiplier;
        protected set => damageMultiplier = value;
    }
    public float CurrentHealth => currentHealth;
    public bool IsDead => currentHealth <= 0f;

    public event Action<float> OnHealthChanged;
    public event Action OnDied;

    protected override void Awake()
    {
        base.Awake();
        currentHealth = maxHealth;
    }
    public float CalculateReceivedDamage(float baseDamage)
    {
        return Mathf.Max(0f, baseDamage - armor);
    }
    public float CalculateDealtDamage(float baseDamage)
    {
        return baseDamage * (1f + damageMultiplier);
    }

    public void TakeDamage(float baseDamage)
    {
        if (IsDead)
            return;

        SetCurrentHealth(currentHealth - CalculateReceivedDamage(baseDamage));
    }

    public void Heal(float amount)
    {
        if (amount > 0f && !IsDead)
            SetCurrentHealth(currentHealth + amount);
    }

    public void SetCurrentHealth(float value)
    {
        bool wasAlive = !IsDead;
        currentHealth = Mathf.Clamp(value, 0f, maxHealth);
        OnHealthChanged?.Invoke(currentHealth);

        if (wasAlive && IsDead)
            OnDied?.Invoke();
    }

    public void AddMaxHealth(float amount)
    {
        if (amount <= 0f)
            return;

        maxHealth += amount;
        SetCurrentHealth(currentHealth + amount);
    }

    public void AddArmor(float amount)
    {
        armor = Mathf.Max(0f, armor + amount);
    }

    public void AddDamageMultiplier(float amount)
    {
        damageMultiplier += amount;
    }
}
