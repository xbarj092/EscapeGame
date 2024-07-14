using UnityEngine;

public class GroundChecker
{
    private CharacterController2D _controller;
    private JumpHandler _jumpHandler;

    public float LeftGroundTime;
    public bool IsGrounded;

    private const float GROUNDED_RADIUS = .5f;

    public GroundChecker(CharacterController2D controller, JumpHandler jumpHandler)
    {
        _controller = controller;
        _jumpHandler = jumpHandler;
    }

    public void CheckGroundStatus()
    {
        bool wasGrounded = IsGrounded;
        IsGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(_controller.GroundCheck.position, GROUNDED_RADIUS, _controller.WhatIsGround);

        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject != _controller.gameObject)
            {
                IsGrounded = true;
                _jumpHandler.DoubleJumpCharged = true;
            }
        }

        if (wasGrounded && !IsGrounded)
        {
            LeftGroundTime = _jumpHandler.TimeElapsed;
        }
    }
}
