using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "DashEffect", menuName = "Survival/Skills/Effects/Dash")]
public sealed class CharacterSkillDashEffect : CharacterSkillEffectDefinition
{
    [SerializeField, Min(0f)] private float distance = 3f;
    [SerializeField, Min(0.01f)] private float duration = 0.5f;
    [SerializeField] private GameObject vfxPrefab;
    [SerializeField] private CharacterSkillEffectDefinition[] onArriveEffects;

    public override void Execute(CharacterSkillExecutionContext context)
    {
        if (context.Controller == null || context.Caster == null)
            return;

        context.Controller.StartCoroutine(DashRoutine(context));
    }

    private IEnumerator DashRoutine(CharacterSkillExecutionContext context)
    {
        CharacterCtrl caster = context.Caster;
        CharacterController controller = caster.GetComponent<CharacterController>();
        if (controller == null)
            yield break;

        HeroMovement heroMovement = caster.GetComponent<HeroMovement>();
        Vector3 startPosition = caster.transform.position;
        Vector3 dashDirection = Vector3.ProjectOnPlane(caster.transform.forward, Vector3.up).normalized;
        if (dashDirection.sqrMagnitude < 0.0001f)
            dashDirection = Vector3.forward;

        heroMovement?.SetSkillMovementLocked(true);
        try
        {
            CharacterSkillVfxSpawner.Spawn(
                vfxPrefab, caster.transform.position, caster.transform.rotation);
            CameraShake.ShakeDash();
            AudioService.PlayDash(caster.transform.position);
            float elapsed = 0f;
            float speed = distance / duration;
            while (elapsed < duration)
            {
                float step = Mathf.Min(Time.deltaTime, duration - elapsed);
                controller.Move(dashDirection * speed * step);
                elapsed += step;
                yield return null;
            }
        }
        finally
        {
            heroMovement?.SetSkillMovementLocked(false);
        }

        Vector3 endPosition = caster.transform.position;

        foreach (CharacterSkillEffectDefinition effect in onArriveEffects ?? System.Array.Empty<CharacterSkillEffectDefinition>())
            effect?.Execute(context);
    }
}
