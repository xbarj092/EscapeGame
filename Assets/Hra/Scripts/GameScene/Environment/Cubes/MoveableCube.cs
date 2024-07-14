using UnityEngine;

public class MoveableCube : BaseCube
{
    [SerializeField] private Vector2 _force;
    [SerializeField] private float _raycastDistance = 0.51f;

    protected override void HandleAction()
    {
        bool wasKinematic = _cubeRigidbody.isKinematic;
        _cubeRigidbody.isKinematic = false;
        Vector2 moveForce = new(_controller.transform.localScale.x > 0 ? _force.x : -_force.x, 0);
        moveForce = new(wasKinematic ? moveForce.x * 4 : moveForce.x, 0);
        Vector2 direction = moveForce.normalized;
        RaycastHit2D hit = Physics2D.Raycast(_cubeRigidbody.position, direction, _raycastDistance);

        if (hit.collider == null || (hit.collider != null && hit.collider.gameObject == gameObject))
        {
            _cubeRigidbody.AddForce(moveForce, ForceMode2D.Impulse);
        }
    }
}
