using UnityEngine;

public class GroundChecker
{
    private CharacterController2D _controller;
    private JumpHandler _jumpHandler;

    public float LeftGroundTime;
    public bool IsGrounded;

    private const float GROUND_CHECK_DISTANCE = 0.05f;

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
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, GROUND_CHECK_DISTANCE, _controller.WhatIsGround);
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject != _controller.gameObject && !collider.gameObject.CompareTag(GlobalConstants.Tags.TutorialCollider.ToString()))
                {
                    IsGrounded = true;
                    _jumpHandler.DoubleJumpCharged = true;
                    break;
                }
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
