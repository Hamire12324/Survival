using System;
using UnityEngine;

public class CharacterLevel : CharacterAbstract
{
    [SerializeField, Min(1)] private int level = 1;
    [SerializeField, Min(1)] private int experienceToLevel = 100;
    [SerializeField, Min(0)] private int currentExperience;

    public int Level => level;
    public int CurrentExperience => currentExperience;
    public event Action<int, int> OnExperienceChanged;
    public event Action<int> OnLevelUp;

    public virtual void AddExperience(int amount)
    {
        if (amount <= 0) return;
        currentExperience += amount;
        while (currentExperience >= experienceToLevel)
        {
            currentExperience -= experienceToLevel;
            level++;
            ApplyLevelUpReward();
            OnLevelUp?.Invoke(level);
        }
        OnExperienceChanged?.Invoke(currentExperience, experienceToLevel);
    }

    protected virtual void ApplyLevelUpReward() { }
}
