using UnityEngine;

public class EnemyTargetFinder : CharacterTargetFinder
{
    protected override void ResetValue()
    {
        base.ResetValue();
        targetLayers = LayerMask.GetMask("Hero");
    }
}
