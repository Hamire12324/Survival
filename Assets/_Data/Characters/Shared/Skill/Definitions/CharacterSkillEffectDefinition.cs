using UnityEngine;

public abstract class CharacterSkillEffectDefinition : ScriptableObject
{
    public abstract void Execute(CharacterSkillExecutionContext context);

    public virtual void ExecuteAtPosition(CharacterSkillExecutionContext context, Vector3 position)
    {
        Execute(context);
    }

    // Lets runtime objects (for example, a delayed bomb) configure their area gizmo
    // from the same gameplay data that will later be used to deal damage.
    public virtual bool TryGetAreaRadius(out float areaRadius)
    {
        areaRadius = 0f;
        return false;
    }
}
