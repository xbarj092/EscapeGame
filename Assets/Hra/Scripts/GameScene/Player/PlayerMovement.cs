using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController2D _controller;
    public float _horizontalMultiplier = 40f;
    private float _horizontalMove = 0f;

    private void Update()
    {
        if (_controller.IsDashing())
        {
            return;
        }

        _horizontalMove = Input.GetAxisRaw("Horizontal") * _horizontalMultiplier;
        _controller.Move(_horizontalMove * Time.fixedDeltaTime);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            _controller.JumpPressed();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && _controller.IsDashPossible())
        {
            _controller.DashPressed();
        }
    }
}
