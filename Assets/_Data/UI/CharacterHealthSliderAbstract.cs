using UnityEngine;

public abstract class CharacterHealthSliderAbstract : SliderAbstract
{
    [SerializeField] private CharacterCtrl characterCtrl;
    private CharacterStat characterStat;

    protected override void Start()
    {
        base.Start();
        Bind();
    }

    protected override void OnDestroy()
    {
        if (characterStat != null)
            characterStat.OnHealthChanged -= Refresh;

        base.OnDestroy();
    }

    protected override void LoadComponents()
    {
        base.LoadComponents();
        LoadCharacterCtrl();
    }

    private void LoadCharacterCtrl()
    {
        if (characterCtrl != null) return;
        characterCtrl = GetComponentInParent<CharacterCtrl>(true);
        Debug.Log(transform.name + " LoadCharacterCtrl: " + (characterCtrl != null), gameObject);
    }

    private void Bind()
    {
        characterStat = characterCtrl?.CharacterStat;
        if (characterStat == null || slider == null)
            return;

        characterStat.OnHealthChanged += Refresh;
        Refresh(characterStat.CurrentHealth);
    }

    private void Refresh(float currentHealth)
    {
        if (slider == null)
            return;

        slider.interactable = false;
        slider.minValue = 0f;
        slider.maxValue = Mathf.Max(1f, characterStat?.MaxHealth ?? 0f);
        slider.SetValueWithoutNotify(Mathf.Clamp(currentHealth, 0f, slider.maxValue));
    }
}
