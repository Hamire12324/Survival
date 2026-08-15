using UnityEngine;

public class HeroBowCombatController : CharacterCombatController
{
    [SerializeField] private HeroCtrl heroCtrl;
    [SerializeField] private ProjectileDamSender arrowPrefab;
    [Header("Projectile Spawn")]
    [SerializeField] private Transform arrowSpawnPoint;
    [SerializeField] private Vector3 fallbackSpawnOffset = new(0f, 1f, 0.6f);

    [SerializeField, Min(0f)] private float baseProjectileDamage = 10f;

    [Header("Basic Attack Charges")]
    [SerializeField, Min(1)] private int maxBasicAttackCharges = 3;
    [SerializeField, Min(0.01f)] private float chargeRecoveryInterval = 3f;

    private int basicAttackCharges;
    private float nextChargeRecoveryTime;

    public int BasicAttackCharges => basicAttackCharges;
    public int MaxBasicAttackCharges => maxBasicAttackCharges;
    public bool CanShoot => heroCtrl != null && arrowPrefab != null;

    protected override void Awake()
    {
        base.Awake();
        basicAttackCharges = maxBasicAttackCharges;
    }

    protected override void Update()
    {
        RecoverBasicAttackCharges();
    }
    protected override void ResetValue()
    {
        base.ResetValue();

        attackCooldown = 0.5f;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadHeroCtrl();
    }

    protected virtual void LoadHeroCtrl()
    {
        if (this.heroCtrl != null) return;
        this.heroCtrl = GetComponentInParent<HeroCtrl>();
        Debug.Log(transform.name + " LoadHeroCtrl: " + (this.heroCtrl != null));
    }
    public override bool TryBasicAttack()
    {
        if (!CanStartBasicAttack() || basicAttackCharges <= 0 || !CanShoot || !heroCtrl.IsInputEnabled ||
            heroCtrl?.CharacterDamReceiver?.IsDead == true)
            return false;

        HeroBowSkillController skillController = heroCtrl.CharacterSkillController as HeroBowSkillController;
        if (skillController == null || !skillController.TryStartBasicAttack())
            return false;

        // The requirement is intentionally based on the hero's *current forward*.
        // Do not auto-face a target here: a 180-degree input change must still take
        // roughly one second to turn, and shots/dash must use the in-progress forward.
        basicAttackCharges--;
        StartBasicAttackCooldown(attackCooldown);
        if (basicAttackCharges < maxBasicAttackCharges && nextChargeRecoveryTime <= Time.time)
            nextChargeRecoveryTime = Time.time + chargeRecoveryInterval;

        heroCtrl.CharacterAnimation?.PlayAttackAnimation();
        skillController.ExecuteBasicAttack();
        CameraShake.ShakeProjectile();
        AudioService.PlayProjectile(heroCtrl.transform.position);
        return true;
    }

    public bool ShootSpread(float damageMultiplier, float[] angleOffsets)
    {
        if (!CanShoot || angleOffsets == null || angleOffsets.Length == 0)
            return false;

        foreach (float angleOffset in angleOffsets)
            Shoot(damageMultiplier, angleOffset);

        return true;
    }

    public bool ShootSpread(DamageData damageData, float[] angleOffsets)
    {
        if (!CanShoot || damageData == null || angleOffsets == null || angleOffsets.Length == 0)
            return false;

        foreach (float angleOffset in angleOffsets)
        {
            Quaternion rotation = heroCtrl.transform.rotation * Quaternion.Euler(0f, angleOffset, 0f);
            ProjectileDamSender arrow = SpawnArrow(GetSpawnPosition(), rotation);
            if (arrow == null) continue;
            arrow.Configure(heroCtrl, damageData);
        }

        return true;
    }
    public bool Shoot(float damageMultiplier, float angleOffset)
    {
        if (!CanShoot)
            return false;

        Quaternion rotation = heroCtrl.transform.rotation * Quaternion.Euler(0f, angleOffset, 0f);
        ProjectileDamSender arrow = SpawnArrow(GetSpawnPosition(), rotation);
        if (arrow == null) return false;
        arrow.Configure(heroCtrl, new DamageData(baseProjectileDamage * Mathf.Max(0f, damageMultiplier)));
        return true;
    }

    private ProjectileDamSender SpawnArrow(Vector3 position, Quaternion rotation)
    {
        PoolObj pooledPrefab = arrowPrefab.GetComponent<PoolObj>();
        PoolManager poolManager = PoolManager.Instance;
        if (pooledPrefab != null && poolManager != null)
            return poolManager.Spawn(pooledPrefab, position, rotation)?.GetComponent<ProjectileDamSender>();

        return Instantiate(arrowPrefab, position, rotation);
    }
    private void RecoverBasicAttackCharges()
    {
        if (basicAttackCharges >= maxBasicAttackCharges || Time.time < nextChargeRecoveryTime)
            return;

        basicAttackCharges++;
        nextChargeRecoveryTime = basicAttackCharges < maxBasicAttackCharges
            ? Time.time + chargeRecoveryInterval
            : 0f;
    }

    private Vector3 GetSpawnPosition()
    {
        return arrowSpawnPoint != null
            ? arrowSpawnPoint.position
            : heroCtrl.transform.TransformPoint(fallbackSpawnOffset);
    }
}
