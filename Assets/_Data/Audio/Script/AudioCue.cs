using UnityEngine;

[CreateAssetMenu(fileName = "AudioCue", menuName = "Survival/Audio Cue")]
public sealed class AudioCue : ScriptableObject
{
    [SerializeField] private AudioClip clip;
    [SerializeField, Range(0f, 1f)] private float volume = 1f;
    [SerializeField, Min(0.01f)] private float minimumPitch = 1f;
    [SerializeField, Min(0.01f)] private float maximumPitch = 1f;
    [SerializeField, Range(0f, 1f)] private float spatialBlend;

    public AudioClip Clip => clip;
    public float Volume => volume;
    public float SpatialBlend => spatialBlend;
    public float GetPitch() => Random.Range(
        Mathf.Min(minimumPitch, maximumPitch),
        Mathf.Max(minimumPitch, maximumPitch));
}
