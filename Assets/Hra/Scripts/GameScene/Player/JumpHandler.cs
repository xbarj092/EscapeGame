using UnityEngine;

public class JumpHandler
{
    public GroundChecker GroundChecker { get; }

    private readonly CharacterController2D _controller;
    private readonly float _verticalJumpForce = 400f;
    private readonly float _horizontalJumpForce = 400f;

    public bool DoubleJumpCharged;
    public float TimeElapsed { get; private set; }
    private bool _wantsToJump;

    private const float COYOTE_JUMP_OFFSET = 0.1f;

    public JumpHandler(CharacterController2D controller)
    {
        _controller = controller;
        GroundChecker = new GroundChecker(controller, this);
    }

    public void HandleJumping()
    {
        if (!_wantsToJump) return;

        if (_controller.IsOnWall())
        {
            float force = _controller.IsFacingRight() ? -_horizontalJumpForce : _horizontalJumpForce;
            PerformJump(force);
        }
        else if (IsJumpPossible())
        {
            PerformJump();
        }
        else if (IsDoubleJumpPossible())
        {
            PerformDoubleJump();
        }

        _wantsToJump = false;
    }

    private void PerformJump(float horizontalVelocity = 0f)
    {
        GroundChecker.IsGrounded = false;
        _controller.Rigidbody2D.AddForce(new Vector2(horizontalVelocity, _verticalJumpForce));
    }

    private void PerformDoubleJump()
    {
        Vector2 currentVelocity = _controller.Rigidbody2D.velocity;
        currentVelocity.y = 0;
        _controller.Rigidbody2D.velocity = currentVelocity;

        _controller.Rigidbody2D.AddForce(new Vector2(0f, _verticalJumpForce));
        DoubleJumpCharged = false;
    }

    public void SetWantsToJump(bool wantsToJump) => _wantsToJump = wantsToJump;
    public void UpdateTimeElapsed(float deltaTime) => TimeElapsed += deltaTime;

    private bool IsCoyoteJumpPossible() => !GroundChecker.IsGrounded && TimeElapsed < GroundChecker.LeftGroundTime + COYOTE_JUMP_OFFSET;
    private bool IsDoubleJumpPossible() => _controller.CanDoubleJump && !GroundChecker.IsGrounded && DoubleJumpCharged;
    private bool IsJumpPossible() => _controller.CanJump && GroundChecker.IsGrounded || IsCoyoteJumpPossible() || _controller.IsWallCoyoteJumpPossible();
}
