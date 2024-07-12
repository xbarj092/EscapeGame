using UnityEngine;

public abstract class BaseCube : MonoBehaviour
{
    protected Rigidbody2D _rigidbody;

    protected abstract void HandleAction();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalConstants.Tags.Player.ToString()))
        {
            _rigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            // TODO: player needs to be dashing for action to occur
            if (true)
            {
                HandleAction();
            }
        }
    }
}
