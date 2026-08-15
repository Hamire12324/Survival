using UnityEngine;

public class HeroBowSkillController : CharacterSkillController
{
    private static readonly float[] MultishotAngles = { -15f, 0f, 15f };
    private static readonly float[] VolleyAngles = { -24f, -12f, 0f, 12f, 24f };

    [SerializeField] private HeroCtrl hero;
    [SerializeField] private HeroBowCombatController bowCombat;
    [SerializeField] private CharacterSkillDefinition basicAttackSkill;
    [Header("Definition Skills")]
    [SerializeField] private CharacterSkillDefinition bombSkill;
    [SerializeField] private CharacterSkillDefinition dashExplosionSkill;

    [Header("Legacy Skills")]
    [SerializeField, Min(0f)] private float volleyCooldown = 7f;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadHeroCtrl();
        LoadBowCombatController();
    }

    private void LoadHeroCtrl()
    {
        hero ??= GetComponentInParent<HeroCtrl>();
    }

    private void LoadBowCombatController()
    {
        bowCombat ??= hero != null
            ? hero.GetComponentInChildren<HeroBowCombatController>(true)
            : GetComponentInChildren<HeroBowCombatController>(true);
    }

    public override bool TryUseSkill(int index)
    {
        if (hero?.CharacterDamReceiver?.IsDead == true || !CanUseSkill(index))
            return false;

        switch (index)
        {
            case 0:
                return TryUseDefinitionSkill(index, bombSkill);
            case 1:
                return TryUseDefinitionSkill(index, dashExplosionSkill);
            case 2:
                return TryFireSkill(index, 1f, MultishotAngles, 5f);
            case 3:
                return TryFireSkill(index, 1.25f, VolleyAngles, volleyCooldown);
            default:
                return false;
        }
    }

    public bool TryStartBasicAttack()
    {
        return hero?.CharacterDamReceiver?.IsDead != true &&
               TryStartDefinitionSkill(4, basicAttackSkill);
    }

    public void ExecuteBasicAttack()
    {
        ExecuteDefinitionSkill(basicAttackSkill);
    }

    private bool TryFireSkill(int index, float damageMultiplier, float[] angles, float cooldown)
    {
        if (bowCombat == null || !bowCombat.ShootSpread(damageMultiplier, angles))
            return false;

        hero?.CharacterAnimation?.PlayAttackAnimation();
        StartSkillCooldown(index, cooldown);
        return true;
    }

}
