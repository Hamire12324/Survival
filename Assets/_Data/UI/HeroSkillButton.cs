using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeroSkillButton : BaseMonoBehaviour
{
    [SerializeField] private HeroCtrl heroCtrl;
    [SerializeField, Min(0)] private int skillIndex;
    [Header("UI")]
    [SerializeField] private Image cooldownFill;
    [SerializeField] private TextMeshProUGUI cooldownText;
    [SerializeField] private Button skillButton;

    protected override void Update()
    {
        if (heroCtrl == null)
            return;

        float remaining = heroCtrl.CharacterSkillController.GetCooldownRemaining(skillIndex);
        bool isCoolingDown = remaining > 0f;

        if (cooldownFill != null)
        {
            float duration = heroCtrl.CharacterSkillController.GetCooldownDuration(skillIndex);
            cooldownFill.fillAmount = duration > 0f ? remaining / duration : 0f;
            cooldownFill.gameObject.SetActive(isCoolingDown);
        }

        if (cooldownText != null)
        {
            cooldownText.text = isCoolingDown ? Mathf.CeilToInt(remaining).ToString() : string.Empty;
            cooldownText.gameObject.SetActive(isCoolingDown);
        }

        if (skillButton != null)
            skillButton.interactable = heroCtrl.IsInputEnabled && !isCoolingDown;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadHeroCtrl();
        this.LoadSkillButton();
        this.LoadSkillText();
        this.LoadCooldownFill();
    }
    protected virtual void LoadHeroCtrl()
    {
        if (heroCtrl != null) return;
        heroCtrl = FindAnyObjectByType<HeroCtrl>();
    }
    protected virtual void LoadSkillButton()
    {
        if (skillButton != null) return;
        skillButton = GetComponent<Button>();
    }
    protected virtual void LoadSkillText()
    {
        if (cooldownText != null) return;
        cooldownText = GetComponentInChildren<TextMeshProUGUI>();
    }
    protected virtual void LoadCooldownFill()
    {
        if (cooldownFill != null) return;
        cooldownFill = transform.Find("CooldownOverlay").GetComponent<Image>();
    }
}
