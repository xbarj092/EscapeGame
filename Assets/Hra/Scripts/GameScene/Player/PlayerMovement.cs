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
        _time += Time.deltaTime;
        horizontalMove = Input.GetAxisRaw("Horizontal") * horizontalMultiplier;

        if (Input.GetButtonDown("Jump")) 
        {
            controller.JumpPressed();
        }
    }

    private void FixedUpdate()
    {
        controller.Move(horizontalMove * Time.fixedDeltaTime);
    }
}
