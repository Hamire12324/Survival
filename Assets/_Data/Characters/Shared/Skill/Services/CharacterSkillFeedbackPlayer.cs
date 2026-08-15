using UnityEngine;

public static class CharacterSkillFeedbackPlayer
{
    public static GameObject CreateBombVfx(Vector3 position)
    {
        GameObject bomb = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        bomb.name = "Skill Bomb VFX";
        bomb.transform.SetPositionAndRotation(position + Vector3.up * 0.25f, Quaternion.identity);
        bomb.transform.localScale = Vector3.one * 0.5f;

        Collider bombCollider = bomb.GetComponent<Collider>();
        if (bombCollider != null)
        {
            bombCollider.isTrigger = true;
            bombCollider.enabled = false;
        }

        Renderer renderer = bomb.GetComponent<Renderer>();
        if (renderer != null)
            renderer.material.color = new Color(1f, 0.25f, 0.05f);

        PlayTone(position, 280f, 0.08f, 0.18f);
        return bomb;
    }

    public static void PlayBombArmed(Vector3 position) =>
        PlayTone(position, 280f, 0.08f, 0.18f);

    public static void PlayExplosion(Vector3 position, float radius)
    {
        CreateBurst(position + Vector3.up * 0.15f, Mathf.Max(8, Mathf.CeilToInt(radius * 8f)),
            Mathf.Max(0.4f, radius * 0.2f), new Color(1f, 0.35f, 0.05f));
        PlayTone(position, 95f, 0.35f, 0.6f);
    }

    public static void PlayDash(Vector3 position)
    {
        CreateBurst(position + Vector3.up * 0.1f, 16, 0.15f, new Color(0.25f, 0.75f, 1f));
        PlayTone(position, 620f, 0.12f, 0.2f);
    }

    private static void CreateBurst(Vector3 position, int count, float size, Color color)
    {
        GameObject burst = new("Skill VFX");
        burst.transform.position = position;
        ParticleSystem particles = burst.AddComponent<ParticleSystem>();
        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        ParticleSystem.MainModule main = particles.main;
        main.loop = false;
        main.startLifetime = 0.45f;
        main.startSpeed = 4f;
        main.startSize = size;
        main.startColor = color;
        main.maxParticles = count;
        main.simulationSpace = ParticleSystemSimulationSpace.World;

        ParticleSystem.EmissionModule emission = particles.emission;
        emission.rateOverTime = 0f;
        ParticleSystem.ShapeModule shape = particles.shape;
        shape.shapeType = ParticleSystemShapeType.Sphere;
        shape.radius = 0.1f;

        particles.Emit(count);
        particles.Play();
        Object.Destroy(burst, 1f);
    }

    private static void PlayTone(Vector3 position, float frequency, float duration, float volume)
    {
        const int sampleRate = 44100;
        int sampleCount = Mathf.CeilToInt(duration * sampleRate);
        float[] samples = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            float envelope = 1f - i / (float)sampleCount;
            samples[i] = Mathf.Sin(2f * Mathf.PI * frequency * i / sampleRate) * envelope;
        }

        AudioClip clip = AudioClip.Create("Skill SFX", sampleCount, 1, sampleRate, false);
        clip.SetData(samples, 0);
        GameObject audioObject = new("Skill SFX");
        audioObject.transform.position = position;
        AudioSource source = audioObject.AddComponent<AudioSource>();
        source.spatialBlend = 1f;
        source.volume = volume;
        source.PlayOneShot(clip);

        Object.Destroy(audioObject, duration + 0.1f);
        Object.Destroy(clip, duration + 0.1f);
    }
}
