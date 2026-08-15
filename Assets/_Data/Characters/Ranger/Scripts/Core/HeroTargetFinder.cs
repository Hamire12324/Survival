using UnityEngine;

public sealed class HeroTargetFinder : CharacterTargetFinder
{
    protected override void ResetValue()
    {
        base.ResetValue();
        targetLayers = LayerMask.GetMask("Enemy");
    }
}
