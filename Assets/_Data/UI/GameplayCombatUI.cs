using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameplayCombatUI : MonoBehaviour
{
    [SerializeField] private HeroCtrl hero;
    private readonly List<CooldownBinding> cooldownBindings = new();

    private void Awake() => hero ??= FindAnyObjectByType<HeroCtrl>();

    private void Start()
    {
        BindBasicAttack("Btn_Attack_Basic");
        BindSkill(0, "Btn_Skill_1");
        BindSkill(1, "Btn_Skill_2");
        BindSkill(2, "Btn_Skill_3");
        BindSkill(3, "Btn_Skill_4");
    }

    private void Update()
    {
        if (hero == null)
            return;

        foreach (CooldownBinding binding in cooldownBindings)
            binding.Refresh(hero);
    }

    private void BindBasicAttack(string buttonName)
    {
        Button button = FindButton(buttonName);
        if (button != null)
        {
            button.onClick.AddListener(UseBasicAttack);
            cooldownBindings.Add(new CooldownBinding(button, -1));
        }
    }

    private void BindSkill(int index, string buttonName)
    {
        Button button = FindButton(buttonName);
        if (button != null)
        {
            button.onClick.AddListener(() => UseSkill(index));
            cooldownBindings.Add(new CooldownBinding(button, index));
        }
    }

    private void UseBasicAttack()
    {
        bool used = hero?.CharacterCombatController?.TryBasicAttack() ?? false;
        PlayUiFeedback(used);
    }

    private void UseSkill(int index)
    {
        bool used = hero?.CharacterSkillController?.TryUseSkill(index) ?? false;
        PlayUiFeedback(used);
    }

    private static void PlayUiFeedback(bool used)
    {
        if (used)
            AudioService.PlayUiClick();
        else
            AudioService.PlayUiUnavailable();
    }

    private static Button FindButton(string buttonName)
    {
        foreach (Transform candidate in FindObjectsByType<Transform>(FindObjectsInactive.Include))
        {
            if (candidate.name == buttonName)
                return candidate.GetComponent<Button>();
        }

        return null;
    }

    private sealed class CooldownBinding
    {
        private readonly Button button;
        private readonly int skillIndex;
        private readonly Image cooldownFill;
        private readonly TextMeshProUGUI cooldownText;

        public CooldownBinding(Button button, int skillIndex)
        {
            this.button = button;
            this.skillIndex = skillIndex;
            cooldownFill = button.transform.Find("CooldownOverlay")?.GetComponent<Image>();
            cooldownText = button.GetComponentInChildren<TextMeshProUGUI>(true);
        }

        public void Refresh(HeroCtrl hero)
        {
            if (hero == null || button == null)
                return;

            float remaining;
            float duration;
            bool hasCharges = true;
            if (skillIndex < 0)
            {
                HeroBowCombatController combat = hero.CharacterCombatController as HeroBowCombatController;
                remaining = combat?.GetAttackCooldownRemaining() ?? 0f;
                duration = combat?.AttackCooldown ?? 0f;
                hasCharges = combat == null || combat.BasicAttackCharges > 0;
            }
            else
            {
                CharacterSkillController skills = hero.CharacterSkillController;
                remaining = skills?.GetCooldownRemaining(skillIndex) ?? 0f;
                duration = skills?.GetCooldownDuration(skillIndex) ?? 0f;
            }

            bool coolingDown = remaining > 0f;
            if (cooldownFill != null)
            {
                cooldownFill.fillAmount = duration > 0f ? remaining / duration : 0f;
                cooldownFill.gameObject.SetActive(coolingDown);
            }

            if (cooldownText != null)
            {
                cooldownText.text = coolingDown ? Mathf.CeilToInt(remaining).ToString() : string.Empty;
                cooldownText.gameObject.SetActive(coolingDown);
            }

            bool isAlive = hero.CharacterDamReceiver?.IsDead != true;
            button.interactable = isAlive && !coolingDown && hasCharges;
        }
    }
}
