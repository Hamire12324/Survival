using UnityEngine;
using UnityEngine.InputSystem;

public class HeroMovement : CharacterMovement
{
    [SerializeField] protected HeroCtrl heroCtrl;
    [SerializeField, Min(0f)] private float gravity = 20f;
    [SerializeField] private float groundedVerticalVelocity = -2f;

    private float verticalVelocity;
    private bool skillMovementLocked;
    private bool attackRotationLocked;

    public void SetSkillMovementLocked(bool locked)
    {
        skillMovementLocked = locked;
    }

    public void SetAttackRotationLocked(bool locked)
    {
        attackRotationLocked = locked;

        if (!locked && characterCtrl != null)
            UpdateLookDirection(characterCtrl.transform.forward);
    }

    protected override void Update()
    {
        if (heroCtrl == null || !heroCtrl.IsInputEnabled)
        {
            ClearInput();
            return;
        }

        ReadKeyboardInput();

        Vector3 movement = skillMovementLocked ? Vector3.zero : GetMoveDirection();
        if (!skillMovementLocked)
            UpdateLookDirection(movement);

        ApplyGravity();

        Vector3 velocity = movement * characterCtrl.CharacterStat.MoveSpeed;
        velocity.y = verticalVelocity;

        heroCtrl.CharacterController.Move(velocity * Time.deltaTime);
        if (!attackRotationLocked)
            RotateTowardsLookDirection();
    }

    public void ClearInput()
    {
        SetMoveInput(Vector2.zero);
    }

    private void ApplyGravity()
    {
        if (heroCtrl.CharacterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = groundedVerticalVelocity;
            return;
        }

        verticalVelocity -= gravity * Time.deltaTime;
    }
    protected override void LoadComponents()
    {
        base.LoadComponents();
        this.LoadHeroCtrl();
    }
    private void LoadHeroCtrl()
    {
        if (heroCtrl != null) return;

        heroCtrl = characterCtrl as HeroCtrl;
        Debug.Log(transform.name + ": Load HeroCtrl", gameObject);
    }
    private void ReadKeyboardInput()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            SetMoveInput(ReadGamepadInput());
            return;
        }

        float horizontal = (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed ? 1f : 0f)
                         - (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed ? 1f : 0f);
        float vertical = (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed ? 1f : 0f)
                       - (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed ? 1f : 0f);

        Vector2 keyboardInput = new(horizontal, vertical);
        SetMoveInput(keyboardInput.sqrMagnitude > 0f
            ? keyboardInput
            : ReadGamepadInput());
    }

    private static Vector2 ReadGamepadInput()
    {
        return Gamepad.current != null
            ? Gamepad.current.leftStick.ReadValue()
            : Vector2.zero;
    }
}
