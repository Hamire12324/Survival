using UnityEngine;

public sealed class CameraShake : MonoBehaviour
{
    [System.Serializable]
    private struct ShakeProfile
    {
        [Min(0f)] public float PositionMagnitude;
        [Min(0f)] public float RotationMagnitude;
        [Min(0.01f)] public float Duration;
    }

    private static CameraShake instance;

    [Header("Shake Profiles")]
    [SerializeField] private ShakeProfile projectileShake = new()
    {
        PositionMagnitude = 0.035f,
        RotationMagnitude = 0.35f,
        Duration = 0.06f
    };
    [SerializeField] private ShakeProfile dashShake = new()
    {
        PositionMagnitude = 0.11f,
        RotationMagnitude = 0.9f,
        Duration = 0.13f
    };
    [SerializeField] private ShakeProfile playerHitShake = new()
    {
        PositionMagnitude = 0.08f,
        RotationMagnitude = 0.7f,
        Duration = 0.12f
    };

    private ShakeProfile activeShake;
    private float remainingDuration;

    private void Awake() => instance = this;

    private void LateUpdate()
    {
        if (remainingDuration <= 0f)
            return;

        float normalizedTime = remainingDuration / activeShake.Duration;
        float falloff = normalizedTime * normalizedTime;
        Vector2 offset = Random.insideUnitCircle * (activeShake.PositionMagnitude * falloff);
        transform.position += transform.right * offset.x + transform.up * offset.y;
        transform.rotation *= Quaternion.Euler(0f, 0f,
            Random.Range(-activeShake.RotationMagnitude, activeShake.RotationMagnitude) * falloff);

        remainingDuration -= Time.deltaTime;
    }
    private void OnDestroy()
    {
        if (instance == this)
            instance = null;
    }

    public static void ShakeProjectile() => instance?.StartShake(instance.projectileShake);
    public static void ShakeDash() => instance?.StartShake(instance.dashShake);
    public static void ShakePlayerHit() => instance?.StartShake(instance.playerHitShake);

    private void StartShake(ShakeProfile profile)
    {
        if (profile.Duration <= 0f)
            return;

        if (remainingDuration <= 0f)
            activeShake = profile;
        else
        {
            activeShake.PositionMagnitude = Mathf.Max(activeShake.PositionMagnitude, profile.PositionMagnitude);
            activeShake.RotationMagnitude = Mathf.Max(activeShake.RotationMagnitude, profile.RotationMagnitude);
            activeShake.Duration = Mathf.Max(activeShake.Duration, profile.Duration);
        }

        remainingDuration = Mathf.Max(remainingDuration, profile.Duration);
    }
}
