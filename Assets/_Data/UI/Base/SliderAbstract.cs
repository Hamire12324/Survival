using UnityEngine;
using UnityEngine.UI;

public abstract class SliderAbstract : CameraFacingUIAbstract
{
    [SerializeField] protected Slider slider;

    protected override void LoadComponents()
    {
        base.LoadComponents();

        this.LoadSlider();
    }
    protected virtual void LoadSlider()
    {
        if (this.slider != null) return;
        this.slider = GetComponent<Slider>();
        Debug.Log(transform.name + " LoadSlider: " + (this.slider != null), gameObject);
    }
}
