using UnityEngine;

public class EnemyMeleeDamSender : MeleeDamSender
{
    [SerializeField, Range(1f, 360f)] private float coneAngle = 50f;
    [SerializeField, Min(0.01f)] private float coneRange = 1.3f;
    protected override void ResetValue()
    {
        base.ResetValue();

        this.targetLayer = LayerMask.GetMask("Hero");
    }
    protected override bool IsWithinDamageArea(Collider hitCollider)
    {
        CharacterCtrl target = hitCollider.GetComponentInParent<CharacterCtrl>();
        if (target?.CharacterCollider == null || characterCtrl == null)
            return false;

        Vector3 toTarget = Vector3.ProjectOnPlane(
            target.CharacterCollider.ClosestPoint(characterCtrl.transform.position) - characterCtrl.transform.position,
            Vector3.up);

        return toTarget.sqrMagnitude > 0.0001f &&
               Vector3.Angle(characterCtrl.transform.forward, toTarget) <= coneAngle * 0.5f;
    }

    public void DrawDamageAreaGizmo()
    {
        Transform origin = characterCtrl != null ? characterCtrl.transform : transform.parent;
        if (origin == null)
            return;

        const int segmentCount = 24;
        float halfAngle = coneAngle * 0.5f;
        Vector3 center = origin.position;
        Gizmos.color = new Color(1f, 0.2f, 0.05f, 0.9f);

        Vector3 previousPoint = center + Quaternion.AngleAxis(-halfAngle, Vector3.up) * origin.forward * coneRange;
        Gizmos.DrawLine(center, previousPoint);
        for (int index = 1; index <= segmentCount; index++)
        {
            float angle = Mathf.Lerp(-halfAngle, halfAngle, index / (float)segmentCount);
            Vector3 point = center + Quaternion.AngleAxis(angle, Vector3.up) * origin.forward * coneRange;
            Gizmos.DrawLine(previousPoint, point);
            previousPoint = point;
        }
        Gizmos.DrawLine(center, previousPoint);
    }

    private void OnDrawGizmosSelected()
    {
        DrawDamageAreaGizmo();
    }
}
