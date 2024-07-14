using UnityEngine;

public class JumpHandler
{
    public GroundChecker GroundChecker { get; }

    private readonly CharacterController2D _controller;
    private readonly float _jumpForce = 400f;

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

        if (GroundChecker.IsGrounded)
        {
            PerformJump();
        }
        else if (IsCoyoteJumpPossible())
        {
            PerformJump();
        }
        else if (IsDoubleJumpPossible())
        {
            PerformDoubleJump();
        }

        _wantsToJump = false;
    }

    public void SetWantsToJump(bool wantsToJump) => _wantsToJump = wantsToJump;

    public void UpdateTimeElapsed(float deltaTime) => TimeElapsed += deltaTime;

    private void PerformJump()
    {
        GroundChecker.IsGrounded = false;
        _controller.Rigidbody2D.AddForce(new Vector2(0f, _jumpForce));
    }

    private void PerformDoubleJump()
    {
        var currentVelocity = _controller.Rigidbody2D.velocity;
        currentVelocity.y = 0;
        _controller.Rigidbody2D.velocity = currentVelocity;

        _controller.Rigidbody2D.AddForce(new Vector2(0f, _jumpForce));
        DoubleJumpCharged = false;
    }

    private bool IsCoyoteJumpPossible() => _controller.CanCoyoteJump && !GroundChecker.IsGrounded && 
        TimeElapsed < GroundChecker.LeftGroundTime + COYOTE_JUMP_OFFSET;

    private bool IsDoubleJumpPossible() => _controller.CanDoubleJump && !GroundChecker.IsGrounded && DoubleJumpCharged;
}
