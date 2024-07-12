using UnityEngine;

public enum Direction
{
    Left = 0,
    Right = 1,
    Up = 2,
    Down = 3
}

public class Laser : MonoBehaviour
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Direction _direction;

    private GameObject _laser;
    private LineRenderer _lineRenderer;

    private void Start()
    {
        CreateLaserVisual();
    }

    private void Update()
    {
        FireLaser();
    }

    private void CreateLaserVisual()
    {
        _laser = Instantiate(new GameObject("Laser"), transform);
        _laser.transform.position = transform.position;

        _lineRenderer = _laser.AddComponent<LineRenderer>();
        _lineRenderer.startWidth = 0.1f;
        _lineRenderer.endWidth = 0.1f;
        _lineRenderer.positionCount = 2;
        _lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        _lineRenderer.startColor = Color.red;
        _lineRenderer.endColor = Color.red;

        SpriteRenderer spriteRenderer = _laser.AddComponent<SpriteRenderer>();
        spriteRenderer.sprite = _sprite;
    }

    private void FireLaser()
    {
        Vector2 direction = GetDirection();
        if (direction != Vector2.zero)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
            if (hit.collider != null)
            {
                if (hit.transform.gameObject.CompareTag(GlobalConstants.Tags.Player.ToString()))
                {
                    PlayerEvents.OnPlayerDeathInvoke();
                }
                else
                {
                    gameObject.SetActive(true);
                    UpdateLaserVisual(hit.point);
                }
            }
        }
    }

    private Vector2 GetDirection()
    {
        return _direction switch
        {
            Direction.Left => Vector2.left,
            Direction.Right => Vector2.right,
            Direction.Up => Vector2.up,
            Direction.Down => Vector2.down,
            _ => Vector2.zero,
        };
    }

    private void UpdateLaserVisual(Vector2 endPosition)
    {
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, endPosition);
    }
}
