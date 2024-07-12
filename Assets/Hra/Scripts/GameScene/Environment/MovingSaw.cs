using UnityEngine;

public class MovingSaw : MonoBehaviour
{
    [SerializeField] private Collider2D _collider;
    [SerializeField] private Transform _startTransform;
    [SerializeField] private Transform _endTransform;
    [SerializeField] private float _speed = 1.0f;

    private Vector3 _targetPosition;

    private const float TARGET_POSITION_THRESHOLD = 0.01f;

    private void Start()
    {
        transform.position = _startTransform.position;
        _targetPosition = _endTransform.position;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _targetPosition) < TARGET_POSITION_THRESHOLD)
        {
            _targetPosition = _targetPosition == _startTransform.position ? _endTransform.position : _startTransform.position;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalConstants.Tags.Player.ToString()))
        {
            PlayerEvents.OnPlayerDeathInvoke();
        }
    }
}
