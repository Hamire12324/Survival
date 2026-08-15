using System;
using UnityEngine;

public class EnemyExperienceReward : CharacterAbstract
{
    [SerializeField, Min(0)] private int experienceReward = 30;
    private HeroLevel recipient;
    public event Action<int> OnExperienceGranted;

    protected override void Awake()
    {
        base.Awake();
        if (CharacterCtrl?.CharacterDamReceiver != null)
            CharacterCtrl.CharacterDamReceiver.OnDeath += HandleDeath;
    }

    protected override void OnDestroy()
    {
        if (CharacterCtrl?.CharacterDamReceiver != null)
            CharacterCtrl.CharacterDamReceiver.OnDeath -= HandleDeath;
        base.OnDestroy();
    }

    public void SetRecipient(HeroLevel heroLevel) => recipient = heroLevel;

    private void HandleDeath(CharacterDamReceiver _)
    {
        recipient?.AddExperience(experienceReward);
        OnExperienceGranted?.Invoke(experienceReward);
    }
}
