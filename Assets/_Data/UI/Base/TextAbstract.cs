using TMPro;
using UnityEngine;
using UnityEngine.UI;

public abstract class TextAbstract : CameraFacingUIAbstract
{
    [SerializeField] protected TextMeshProUGUI textMeshProUGUI;

    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadTextMeshProUGUI();
    }

    protected virtual void LoadTextMeshProUGUI()
    {
        if (this.textMeshProUGUI != null) return;
        this.textMeshProUGUI = GetComponent<TextMeshProUGUI>();
        Debug.Log(transform.name + " LoadText: " + (this.textMeshProUGUI != null), gameObject);
    }
}
