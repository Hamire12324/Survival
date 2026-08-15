using UnityEngine;

public class HeroAnimator : CharacterAnimation
{
    private HeroMovement heroMovement;

    protected override void Update()
    {
        heroMovement ??= characterCtrl.GetComponentInChildren<HeroMovement>();

        if (characterCtrl.Animator == null || heroMovement == null)
            return;

        characterCtrl.Animator.SetFloat("Speed", heroMovement.MoveInput.sqrMagnitude);
    }
}