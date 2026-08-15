using System.Collections;
using UnityEngine;

public class VFXPoolObj : PoolObj
{
    [SerializeField, Min(0.01f)] private float fallbackLifetime = 1f;
    private Coroutine returnRoutine;
    public override void OnSpawnedFromPool()
    {
        base.OnSpawnedFromPool();
        foreach (ParticleSystem particle in GetComponentsInChildren<ParticleSystem>(true))
        {
            particle.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            particle.Play(true);
        }
        foreach (AudioSource audioSource in GetComponentsInChildren<AudioSource>(true))
        {
            audioSource.Stop();
            audioSource.Play();
        }
        returnRoutine = StartCoroutine(ReturnAfterPlayback());
    }
    public override void OnReturnedToPool()
    {
        if (returnRoutine != null) StopCoroutine(returnRoutine);
        returnRoutine = null;
        base.OnReturnedToPool();
    }
    private IEnumerator ReturnAfterPlayback()
    {
        yield return new WaitForSeconds(fallbackLifetime);
        ReturnToPool();
    }
}
