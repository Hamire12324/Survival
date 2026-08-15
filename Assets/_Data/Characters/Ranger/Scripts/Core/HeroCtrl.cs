using UnityEngine;

public class HeroCtrl : CharacterCtrl
{
    [SerializeField] private CharacterController characterController;
    public CharacterController CharacterController => characterController;

    private bool inputEnabled = true;
    public bool IsInputEnabled => inputEnabled && CharacterDamReceiver?.IsDead != true;

    public void SetInputEnabled(bool enabled)
    {
        inputEnabled = enabled;

        if (!enabled && CharacterMovement is HeroMovement movement)
            movement.ClearInput();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCharacterController();
        this.EnsureTargetFinder();
        this.EnsureBowSkillController();
        this.EnsureLevel();
    }

    protected virtual void LoadCharacterController()
    {
        if (this.characterController != null) return;
        this.characterController = GetComponent<CharacterController>();
        if (this.characterController == null)
        {
            Debug.LogError($"[HeroCtrl] CharacterController is not found in {gameObject.name}");
        }
    }

    private void EnsureTargetFinder()
    {
        if (characterTargetFinder is HeroTargetFinder)
            return;

        HeroTargetFinder finder = GetComponentInChildren<HeroTargetFinder>(true);
        if (finder == null)
            finder = gameObject.AddComponent<HeroTargetFinder>();

        characterTargetFinder = finder;
    }

    private void EnsureBowSkillController()
    {
        if (CharacterSkillController is HeroBowSkillController)
            return;

        HeroBowSkillController bowSkills = GetComponentInChildren<HeroBowSkillController>(true);
        if (bowSkills == null)
            bowSkills = gameObject.AddComponent<HeroBowSkillController>();

        characterSkillController = bowSkills;
    }

    private void EnsureLevel()
    {
        if (GetComponentInChildren<HeroLevel>(true) == null)
            gameObject.AddComponent<HeroLevel>();
    }
}
