using UnityEngine;

public abstract class BaseCube : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D _cubeRigidbody;

    protected CharacterController2D _controller;

    protected abstract void HandleAction();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalConstants.Tags.Player.ToString()))
        {
            if (CanHandleAction(collision))
            {
                HandleAction();
            }
            else
            {
                _cubeRigidbody.isKinematic = true;
                _cubeRigidbody.velocity = Vector2.zero;
            }
        }
    }

    private bool CanHandleAction(Collision2D collision) => 
        collision.gameObject.TryGetComponent(out _controller) && _controller.IsDashing();
}
