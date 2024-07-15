using UnityEngine;

public class FlipHandler
{
    private readonly CharacterController2D _controller;
    private bool _facingRight = true;

    public FlipHandler(CharacterController2D controller)
    {
        _controller = controller;
    }

    public void CheckFlip(float move)
    {
        if (ShouldFlip(move))
        {
            Flip();
        }
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 theScale = _controller.transform.localScale;
        theScale.x *= -1;
        theScale.y *= -1;
        _controller.transform.localScale = theScale;
    }

    private bool ShouldFlip(float move) => (move > 0 && !_facingRight) || (move < 0 && _facingRight);

    public bool IsFacingRight() => _facingRight;
}
