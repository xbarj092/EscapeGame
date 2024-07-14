using UnityEngine;

public class ClimbHandler
{
    private CharacterController2D _controller;
    public WallChecker WallChecker;

    public float TimeElapsed { get; private set; }
    private bool _wantsToClimb;

    private const float COYOTE_JUMP_OFFSET = 0.1f;

    public ClimbHandler(CharacterController2D controller)
    {
        _controller = controller;

        WallChecker = new(_controller, this);
    }

    public void HandleClimbing(float verticalMove)
    {
    }

    public void SetWantsToClimb(bool wantsToClimb)
    {
        _wantsToClimb = wantsToClimb;
    }

    public void UpdateTimeElapsed(float deltaTime) => TimeElapsed += deltaTime;

    public bool IsCoyoteJumpPossible() => !IsOnWall() && TimeElapsed < WallChecker.LeftWallTime + COYOTE_JUMP_OFFSET;
    public bool CanClimb() => _wantsToClimb && WallChecker.IsOnWall;
    public bool IsOnWall() => WallChecker.IsOnWall;
}
