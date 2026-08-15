using UnityEngine;
public sealed class TopDownCameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 12f, -9f);
    [SerializeField, Min(0f)] private float followSharpness = 12f;

    private void LateUpdate()
    {
        if (target == null)
            target = FindAnyObjectByType<CharacterMovement>()?.transform;

        if (target == null)
            return;

        float t = 1f - Mathf.Exp(-followSharpness * Time.deltaTime);
        transform.position = Vector3.Lerp(transform.position, target.position + offset, t);
        transform.rotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
    }
}
