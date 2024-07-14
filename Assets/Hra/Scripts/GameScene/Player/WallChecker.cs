using UnityEngine;

public class WallChecker
{
    private CharacterController2D _controller;
    private ClimbHandler _climbHandler;

    public float LeftWallTime;
    public bool IsOnWall;

    public WallChecker(CharacterController2D controller, ClimbHandler climbHandler)
    {
        _controller = controller;
        _climbHandler = climbHandler;
    }

    public void CheckWallStatus()
    {
        bool wasOnWall = IsOnWall;
        IsOnWall = false;

        foreach (Transform check in _controller.WallCheckList)
        {
            if (Physics2D.OverlapCircle(check.position, 0.01f, _controller.WhatIsWall))
            {
                IsOnWall = true;
            }
        }

        if (wasOnWall && !IsOnWall)
        {
            LeftWallTime = _climbHandler.TimeElapsed;
        }
    }
}
