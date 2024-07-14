using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D _controller;
    public float _horizontalMultiplier = 40f;
    public float _verticalMultiplier = 20f;
    private float _horizontalMove = 0f;
    private float _verticalMove = 0f;

    private void Update()
    {
        if (_controller.IsDashing()) return;

        _horizontalMove = Input.GetAxisRaw("Horizontal") * _horizontalMultiplier;
        _verticalMove = Input.GetAxisRaw("Vertical") * _verticalMultiplier;

        if (_verticalMove != 0f)
        {
            _controller.ClimbPressed();
            _controller.Move(_verticalMove * Time.deltaTime, false);
            PlayerEvents.OnPlayerClimbedInvoke();
        }
        else
        {
            _controller.ClimbReleased();
        }

        _controller.Move(_horizontalMove * Time.deltaTime);

        if (_horizontalMove != 0f)
        {
            PlayerEvents.OnPlayerMovedInvoke();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _controller.JumpPressed();
            PlayerEvents.OnPlayerJumpedInvoke();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && _controller.IsDashPossible())
        {
            _controller.DashPressed();
            PlayerEvents.OnPlayerDashedInvoke();
        }
    }
}
