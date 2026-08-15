using UnityEngine;

public class PlayerLevelText : TextAbstract
{
    [SerializeField] private CharacterCtrl characterCtrl;
    protected override void Start()
    {
        base.Start();
        Bind();
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadCharacterCtrl();
    }
    protected virtual void LoadCharacterCtrl()
    {
        if (this.characterCtrl != null) return;
        this.characterCtrl = GetComponentInParent<CharacterCtrl>(true);
        Debug.Log(transform.name + " LoadCharacterCtrl: " + (this.characterCtrl != null), gameObject);
    }
    private void Bind()
    {
        characterCtrl.CharacterLevel.OnLevelUp -= Refresh;
        characterCtrl.CharacterLevel.OnLevelUp += Refresh;
        Refresh(characterCtrl.CharacterLevel.Level);
    }

    private void Refresh(int level)
    {
        if (textMeshProUGUI != null)
            textMeshProUGUI.text = $"Level {level}";
    }

    protected override void OnDestroy()
    {
        if (characterCtrl?.CharacterLevel != null)
            characterCtrl.CharacterLevel.OnLevelUp -= Refresh;

        base.OnDestroy();
    }
}
