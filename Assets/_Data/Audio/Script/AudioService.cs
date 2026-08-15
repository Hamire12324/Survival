using System.Collections.Generic;
using UnityEngine;
public sealed class AudioService : MonoBehaviour
{
    private static AudioService instance;

    [Header("Pooled Sources")]
    [SerializeField, Min(1)] private int poolSize = 12;
    [SerializeField] private Transform poolParent;

    [Header("Gameplay Cues")]
    [SerializeField] private AudioCue projectileCue;
    [SerializeField] private AudioCue hitCue;
    [SerializeField] private AudioCue dashCue;
    [SerializeField] private AudioCue bombPlacedCue;
    [SerializeField] private AudioCue explosionCue;
    [SerializeField] private AudioCue uiClickCue;
    [SerializeField] private AudioCue uiUnavailableCue;
    [SerializeField] private AudioCue levelUpCue;

    private readonly List<AudioSource> sources = new();
    private int nextSourceIndex;

    private void Awake()
    {
        instance = this;
        Prewarm();
    }

    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public static void PlayProjectile(Vector3 position) => instance?.Play(instance.projectileCue, position);
    public static void PlayHit(Vector3 position) => instance?.Play(instance.hitCue, position);
    public static void PlayDash(Vector3 position) => instance?.Play(instance.dashCue, position);
    public static void PlayBombPlaced(Vector3 position) => instance?.Play(instance.bombPlacedCue, position);
    public static void PlayExplosion(Vector3 position) => instance?.Play(instance.explosionCue, position);
    public static void PlayUiClick() => instance?.Play(instance.uiClickCue, Vector3.zero);
    public static void PlayUiUnavailable() => instance?.Play(instance.uiUnavailableCue, Vector3.zero);
    public static void PlayLevelUp() => instance?.Play(instance.levelUpCue, Vector3.zero);

    private void Prewarm()
    {
        for (int i = sources.Count; i < poolSize; i++)
        {
            GameObject sourceObject = new($"Pooled SFX {i + 1}");
            sourceObject.transform.SetParent(poolParent != null ? poolParent : transform);
            AudioSource source = sourceObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            sources.Add(source);
        }
    }

    private void Play(AudioCue cue, Vector3 position)
    {
        if (cue == null || cue.Clip == null || sources.Count == 0)
            return;

        AudioSource source = GetAvailableSource();
        source.transform.position = position;
        source.clip = cue.Clip;
        source.volume = cue.Volume;
        source.pitch = cue.GetPitch();
        source.spatialBlend = cue.SpatialBlend;
        source.Play();
    }

    private AudioSource GetAvailableSource()
    {
        for (int offset = 0; offset < sources.Count; offset++)
        {
            int index = (nextSourceIndex + offset) % sources.Count;
            if (!sources[index].isPlaying)
            {
                nextSourceIndex = (index + 1) % sources.Count;
                return sources[index];
            }
        }

        AudioSource source = sources[nextSourceIndex];
        nextSourceIndex = (nextSourceIndex + 1) % sources.Count;
        source.Stop();
        return source;
    }
}
