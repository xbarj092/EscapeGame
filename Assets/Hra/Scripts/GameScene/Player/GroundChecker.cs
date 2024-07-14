using UnityEngine;

public class GroundChecker
{
    private CharacterController2D _controller;
    private JumpHandler _jumpHandler;

    public float LeftGroundTime;
    public bool IsGrounded;

    private const float GROUND_CHECK_DISTANCE = 0.1f;

    public GroundChecker(CharacterController2D controller, JumpHandler jumpHandler)
    {
        _controller = controller;
        _jumpHandler = jumpHandler;
    }

    public void CheckGroundStatus()
    {
        bool wasGrounded = IsGrounded;
        IsGrounded = false;

        foreach (Transform transform in _controller.GroundCheckList)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, GROUND_CHECK_DISTANCE, _controller.WhatIsGround);
            if (hit.collider != null && hit.collider.gameObject != _controller.gameObject)
            {
                IsGrounded = true;
                _jumpHandler.DoubleJumpCharged = true;
            }
        }

        if (_controller.IsOnWall())
        {
            _jumpHandler.DoubleJumpCharged = true;
        }

        if (wasGrounded && !IsGrounded)
        {
            LeftGroundTime = _jumpHandler.TimeElapsed;
        }
    }
}
