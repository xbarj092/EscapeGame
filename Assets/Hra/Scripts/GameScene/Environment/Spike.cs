using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalConstants.Tags.Player.ToString()))
        {
            PlayerEvents.OnPlayerDeathInvoke();
        }
    }
}
