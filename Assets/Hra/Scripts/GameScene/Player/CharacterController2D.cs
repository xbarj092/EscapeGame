using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class CharacterController2D : MonoBehaviour
{
    private FlipHandler _flipHandler;
    private DashHandler _dashHandler;
    private JumpHandler _jumpHandler;

    [SerializeField] private float _movementSmoothing = 0.05f;
    [field: SerializeField] public LayerMask WhatIsGround { get; private set; }
    [field: SerializeField] public Transform GroundCheck { get; private set; }
    [HideInInspector] public Rigidbody2D Rigidbody2D;

    [Header("TOGGLE ABILITIES")]
    public bool CanDash = false;
    public bool CanCoyoteJump = false;
    public bool CanDoubleJump = false;

    private Vector3 _velocity = Vector3.zero;

    private void Awake()
    {
        Rigidbody2D = GetComponent<Rigidbody2D>();

        _flipHandler = new(this);
        _dashHandler = new(this);
        _jumpHandler = new(this);
    }

    private void FixedUpdate()
    {
        if (_dashHandler.IsDashing) return;

        _jumpHandler.UpdateTimeElapsed(Time.deltaTime);
        _jumpHandler.GroundChecker.CheckGroundStatus();
    }

    public void Move(float move)
    {
        if (_dashHandler.IsDashing) return;

        ApplyMovement(move);
        _jumpHandler.HandleJumping();
    }

    private void ApplyMovement(float move)
    {
        Vector3 targetVelocity = new(move * 2f, Rigidbody2D.velocity.y);
        Rigidbody2D.velocity = Vector3.SmoothDamp(Rigidbody2D.velocity, targetVelocity, ref _velocity, _movementSmoothing);

        _flipHandler.CheckFlip(move);
    }

    public void JumpPressed() => _jumpHandler.SetWantsToJump(true);

    public void DashPressed() => _dashHandler.DashPressed();

    public bool IsDashing() => _dashHandler.IsDashing;

    public bool IsDashPossible() => _dashHandler.DashPossible;

    public float TimeToNextDash() => _dashHandler.TimeToNextDash();
}
