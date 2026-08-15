using System.Collections;
using TMPro;
using UnityEngine;

public sealed class LevelUpPopup : MonoBehaviour
{
    [SerializeField] private HeroLevel heroLevel;
    [SerializeField, Min(0.1f)] private float displayDuration = 1.15f;

    private TextMeshProUGUI text;
    private Coroutine routine;

    private void Awake()
    {
        heroLevel ??= GetComponentInChildren<HeroLevel>(true);
        CreateText();
    }

    private void OnEnable()
    {
        if (heroLevel != null)
            heroLevel.OnLevelUp += Show;
    }

    private void OnDisable()
    {
        if (heroLevel != null)
            heroLevel.OnLevelUp -= Show;
    }

    private void CreateText()
    {
        if (text != null)
            return;

        Canvas canvas = FindScreenSpaceCanvas();
        if (canvas == null)
            return;

        GameObject popup = new("Level Up Popup", typeof(RectTransform), typeof(CanvasRenderer),
            typeof(TextMeshProUGUI));
        RectTransform rect = popup.GetComponent<RectTransform>();
        rect.SetParent(canvas.transform, false);
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.62f);
        rect.sizeDelta = new Vector2(650f, 170f);

        text = popup.GetComponent<TextMeshProUGUI>();
        text.font = TMP_Settings.defaultFontAsset;
        text.alignment = TextAlignmentOptions.Center;
        text.fontSize = 66f;
        text.fontStyle = FontStyles.Bold;
        text.color = new Color(1f, 0.83f, 0.12f, 1f);
        popup.SetActive(false);
    }

    private void Show(int level)
    {
        if (text == null)
            CreateText();
        if (text == null)
            return;

        if (routine != null)
            StopCoroutine(routine);

        text.text = $"LEVEL UP!\n<size=42>LEVEL {level}</size>";
        text.gameObject.SetActive(true);
        routine = StartCoroutine(ShowRoutine());
    }

    private IEnumerator ShowRoutine()
    {
        float elapsed = 0f;
        while (elapsed < displayDuration)
        {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / displayDuration);
            float alpha = progress < 0.72f ? 1f : 1f - (progress - 0.72f) / 0.28f;
            text.rectTransform.localScale = Vector3.one * (1f + Mathf.Sin(progress * Mathf.PI) * 0.15f);
            Color color = text.color;
            color.a = alpha;
            text.color = color;
            yield return null;
        }

        text.gameObject.SetActive(false);
        routine = null;
    }

    private static Canvas FindScreenSpaceCanvas()
    {
        foreach (Canvas canvas in FindObjectsByType<Canvas>(FindObjectsInactive.Include))
        {
            if (canvas.renderMode != RenderMode.WorldSpace)
                return canvas;
        }

        return null;
    }
}
