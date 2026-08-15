using UnityEngine;
public class CharacterSkillController : CharacterAbstract
{
    [SerializeField, Min(0)] private int skillSlotCount = 4;
    private float[] nextUseTimes;
    private float[] cooldownDurations;

    public virtual bool TryUseSkill(int index) => false;

    public virtual float GetCooldownRemaining(int index)
    {
        return IsValidSkillIndex(index) ? Mathf.Max(0f, nextUseTimes[index] - Time.time) : 0f;
    }

    public float GetCooldownDuration(int index) =>
        IsValidSkillIndex(index) ? cooldownDurations[index] : 0f;

    protected bool CanUseSkill(int index)
    {
        return IsValidSkillIndex(index) && Time.time >= nextUseTimes[index];
    }

    protected void StartSkillCooldown(int index, float cooldown)
    {
        if (IsValidSkillIndex(index))
        {
            cooldownDurations[index] = Mathf.Max(0f, cooldown);
            nextUseTimes[index] = Time.time + cooldownDurations[index];
        }
    }

    protected bool TryUseDefinitionSkill(int index, CharacterSkillDefinition definition)
    {
        if (!TryStartDefinitionSkill(index, definition))
            return false;

        ExecuteDefinitionSkill(definition);
        return true;
    }

    protected bool TryStartDefinitionSkill(int index, CharacterSkillDefinition definition)
    {
        if (!CanUseSkill(index) || definition == null || characterCtrl == null ||
            characterCtrl.CharacterDamReceiver?.IsDead == true)
            return false;

        StartSkillCooldown(index, definition.Cooldown);
        return true;
    }

    protected void ExecuteDefinitionSkill(CharacterSkillDefinition definition)
    {
        if (definition == null || characterCtrl == null)
            return;

        CharacterSkillExecutionContext context = new(this, definition);
        foreach (CharacterSkillEffectDefinition effect in definition.Effects)
            effect?.Execute(context);
    }

    private bool IsValidSkillIndex(int index)
    {
        EnsureCooldownSlots();
        return index >= 0 && index < nextUseTimes.Length;
    }

    private void EnsureCooldownSlots()
    {
        int count = Mathf.Max(0, skillSlotCount);
        if (nextUseTimes != null && nextUseTimes.Length == count) return;

        float[] previous = nextUseTimes;
        nextUseTimes = new float[count];
        float[] previousDurations = cooldownDurations;
        cooldownDurations = new float[count];
        if (previous != null)
            System.Array.Copy(previous, nextUseTimes, Mathf.Min(previous.Length, nextUseTimes.Length));
        if (previousDurations != null)
            System.Array.Copy(previousDurations, cooldownDurations,
                Mathf.Min(previousDurations.Length, cooldownDurations.Length));
    }
}
