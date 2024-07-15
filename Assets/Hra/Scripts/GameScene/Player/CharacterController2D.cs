using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    private FlipHandler _flipHandler;
    private DashHandler _dashHandler;
    private JumpHandler _jumpHandler;
    private ClimbHandler _climbHandler;

    [field: SerializeField] public float MovementSmoothing = 0.05f;

    [field: SerializeField] public LayerMask WhatIsGround { get; private set; }
    [field: SerializeField] public List<Transform> GroundCheckList { get; private set; } = new();

    [field: SerializeField] public LayerMask WhatIsWall { get; private set; }
    [field: SerializeField] public List<Transform> WallCheckList { get; private set; } = new();
    [field: SerializeField] public TrailRenderer tr;


    [HideInInspector] public Rigidbody2D Rigidbody2D;
    [HideInInspector] public Vector3 Velocity = Vector3.zero;

    public bool CanMove = true;

    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();

        _flipHandler = new(this);
        _dashHandler = new(this, tr);
        _jumpHandler = new(this);
        _climbHandler = new(this);
    }

    private void FixedUpdate()
    {
        if (_dashHandler.IsDashing) return;

        _climbHandler.UpdateTimeElapsed(Time.deltaTime);
        _climbHandler.WallChecker.CheckWallStatus();

        _jumpHandler.UpdateTimeElapsed(Time.deltaTime);
        _jumpHandler.GroundChecker.CheckGroundStatus();
    }

    public void Move(float move, bool horizontal = true)
    {
        if (_dashHandler.IsDashing || !CanMove) return;

        if (horizontal)
        {
            ApplyMovement(move, true);
        }
        else if (_climbHandler.CanClimb())
        {
            ApplyMovement(move, false);
        }

        _jumpHandler.HandleJumping();
    }

    public void ApplyMovement(float move, bool horizontal)
    {
        Vector3 targetVelocity = horizontal ? new Vector3(move * 2f, Rigidbody2D.velocity.y) : new Vector3(0, move * 2f);
        Rigidbody2D.velocity = Vector3.SmoothDamp(Rigidbody2D.velocity, targetVelocity, ref Velocity, MovementSmoothing);

        if (horizontal)
        {
            _flipHandler.CheckFlip(move);
        }
    }

    public void JumpPressed() => _jumpHandler.SetWantsToJump(true);
    public void DashPressed() => _dashHandler.DashPressed();
    public void ClimbPressed() => _climbHandler.SetWantsToClimb(true);
    public void ClimbReleased() => _climbHandler.SetWantsToClimb(false);

    public bool IsFacingRight() => _flipHandler.IsFacingRight();
    public bool IsWallCoyoteJumpPossible() => _climbHandler.IsCoyoteJumpPossible();
    public bool IsOnWall() => _climbHandler.IsOnWall();
    public bool IsDashing() => _dashHandler.IsDashing;
    public bool IsDashPossible() => GameManager.Instance.CanDash && _dashHandler.IsDashPossible();
    public float TimeToNextDash() => _dashHandler.TimeToNextDash();
}
