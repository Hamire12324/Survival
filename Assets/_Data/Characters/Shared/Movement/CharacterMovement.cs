using UnityEngine;
public abstract class CharacterMovement : CharacterAbstract
{
    protected Vector2 moveInput;
    protected Vector3 lookDirection = Vector3.forward;

    public Vector2 MoveInput => moveInput;
    public Vector3 LookDirection => lookDirection;

    protected void SetMoveInput(Vector2 input)
    {
        moveInput = Vector2.ClampMagnitude(input, 1f);
    }

    protected Vector3 GetMoveDirection()
    {
        return new Vector3(moveInput.x, 0f, moveInput.y);
    }

    protected void UpdateLookDirection(Vector3 direction)
    {
        if (direction.sqrMagnitude > 0.0001f)
            lookDirection = direction.normalized;
    }

    protected void RotateTowardsLookDirection()
    {
        if (characterCtrl == null || lookDirection.sqrMagnitude < 0.0001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        characterCtrl.transform.rotation = Quaternion.RotateTowards(
            characterCtrl.transform.rotation,
            targetRotation,
            characterCtrl.CharacterStat.RotationSpeed * Time.deltaTime);
    }
}
