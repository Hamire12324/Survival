using UnityEngine;

public abstract class CameraFacingUIAbstract : BaseMonoBehaviour
{
    private Canvas parentCanvas;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        parentCanvas ??= GetComponentInParent<Canvas>();
    }

    protected override void LateUpdate()
    {
        if (parentCanvas == null || parentCanvas.renderMode != RenderMode.WorldSpace || Camera.main == null)
            return;

        parentCanvas.transform.rotation = Camera.main.transform.rotation;
    }
}
