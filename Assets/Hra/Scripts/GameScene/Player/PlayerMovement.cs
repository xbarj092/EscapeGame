using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public float horizontalMultiplier = 40f;
    private float horizontalMove = 0f;
    private bool jump = false;

    void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal") * horizontalMultiplier;

        if (Input.GetButtonDown("Jump")) jump = true;
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime, false, jump);
        jump = false;
    }
}
