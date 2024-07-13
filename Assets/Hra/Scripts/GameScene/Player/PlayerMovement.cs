using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public float horizontalMultiplier = 40f;
    private float horizontalMove = 0f;
    private bool jump = false;
    private float _time;

    void Update()
    {
        if (controller.isDashing)
        {
            return;
        }
        _time += Time.deltaTime;
        horizontalMove = Input.GetAxisRaw("Horizontal") * horizontalMultiplier;

        if (Input.GetButtonDown("Jump")) 
        {
            controller.JumpPressed();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && controller.dashPossible)
        {
            controller.DashPressed();
        }
    }

    private void FixedUpdate()
    {
        if (controller.isDashing)
        {
            return;
        }
        controller.Move(horizontalMove * Time.fixedDeltaTime);
    }
}
