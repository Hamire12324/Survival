using System;
using UnityEngine;
public abstract class CharacterCtrl : BaseMonoBehaviour
{
    [SerializeField] protected Faction faction;
    public Faction Faction => faction;
    [SerializeField] protected Transform model;
    public Transform Model => model;
    [SerializeField] private Animator animator;
    public Animator Animator => animator;
    [SerializeField] private Collider characterCollider;
    public Collider CharacterCollider => characterCollider;
    [SerializeField] private CharacterMovement characterMovement;
    public CharacterMovement CharacterMovement => characterMovement;
    [SerializeField] private CharacterAnimation characterAnimation;
    public CharacterAnimation CharacterAnimation => characterAnimation;
    [SerializeField] private CharacterDamSender characterDamSender;
    public CharacterDamSender CharacterDamSender => characterDamSender;
    [SerializeField] private CharacterDamReceiver characterDamReceiver;
    public CharacterDamReceiver CharacterDamReceiver => characterDamReceiver;
    [SerializeField] private CharacterStat characterStat;
    public CharacterStat CharacterStat => characterStat;
    [SerializeField] private CharacterCombatController characterCombatController;
    public CharacterCombatController CharacterCombatController => characterCombatController;
    [SerializeField] protected CharacterSkillController characterSkillController;
    public CharacterSkillController CharacterSkillController => characterSkillController;
    [SerializeField] protected CharacterTargetFinder characterTargetFinder;
    public CharacterTargetFinder CharacterTargetFinder => characterTargetFinder;
    [SerializeField] protected CharacterLevel characterLevel;
    public CharacterLevel CharacterLevel => characterLevel;
    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadModel();
        this.LoadAnimator();
        this.LoadCharacterMovement();
        this.LoadCharacterAnimation();
        this.LoadCharacterDamReceiver();
        this.LoadCharacterDamSender();
        this.LoadCharacterCollider();
        this.LoadCharacterStat();
        this.LoadCharacterCombatController();
        this.LoadCharacterSkillController();
        this.LoadTargetFinder();
        this.LoadCharacterLevel();
    }
    protected virtual void LoadModel()
    {
        this.model = transform.Find("Model");
    }
    protected virtual void LoadAnimator()
    {
        this.animator = GetComponentInChildren<Animator>();
    }
    protected virtual void LoadCharacterCollider()
    {
        if (characterCollider != null) return;

        characterCollider = GetComponent<Collider>();
        if (characterCollider == null)
            characterCollider = CharacterDamReceiver?.GetComponentInChildren<Collider>(true);
    }
    protected virtual void LoadCharacterMovement()
    {
        if (this.characterMovement != null) return;
        this.characterMovement = GetComponentInChildren<CharacterMovement>();
        Debug.Log(transform.name + ": LoadCharacterMovement", gameObject);
    }
    protected virtual void LoadCharacterAnimation()
    {
        if (this.characterAnimation != null) return;
        this.characterAnimation = GetComponentInChildren<CharacterAnimation>();
        Debug.Log(transform.name + ": LoadChracterAnimation", gameObject);
    }
    protected virtual void LoadCharacterDamReceiver()
    {
        if (this.characterDamReceiver != null) return;
        this.characterDamReceiver = GetComponentInChildren<CharacterDamReceiver>();
        Debug.Log(transform.name + ": LoadCharacterDamReceiver", gameObject);
    }
    protected virtual void LoadCharacterDamSender()
    {
        if (this.characterDamSender != null) return;
        this.characterDamSender = GetComponentInChildren<CharacterDamSender>();
        //Debug.Log(transform.name + ": LoadCharacterDamSender", gameObject);
    }
    protected virtual void LoadCharacterStat()
    {
        if (this.characterStat != null) return;
        this.characterStat = GetComponentInChildren<CharacterStat>();
        Debug.Log(transform.name + ": LoadCharacterStat", gameObject);
    }
    protected virtual void LoadCharacterCombatController()
    {
        if (this.characterCombatController != null) return;
        this.characterCombatController = GetComponentInChildren<CharacterCombatController>(true);
    }
    protected virtual void LoadCharacterSkillController()
    {
        if (this.characterSkillController != null) return;
        this.characterSkillController = GetComponentInChildren<CharacterSkillController>(true);
    }
    protected virtual void LoadTargetFinder()
    {
        if (characterTargetFinder != null) return;
        characterTargetFinder = GetComponentInChildren<CharacterTargetFinder>(true);
    }
    protected virtual void LoadCharacterLevel()
    {
        if (characterLevel != null) return;
        characterLevel = GetComponentInChildren<CharacterLevel>(true);
    }
}
