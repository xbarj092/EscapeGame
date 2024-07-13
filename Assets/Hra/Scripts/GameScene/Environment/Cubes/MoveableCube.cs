using UnityEngine;

public class MoveableCube : BaseCube
{
    [SerializeField] private Rigidbody2D _cubeRigidbody;
    [SerializeField] private Vector2 _force;
    [SerializeField] private float _raycastDistance = 0.51f;

    protected override void HandleAction()
    {
        Vector2 moveForce = new(_cubeRigidbody.velocity.x > 0 ? _force.x : -_force.x, 0);
        Vector2 direction = moveForce.normalized;
        RaycastHit2D hit = Physics2D.Raycast(_cubeRigidbody.position, direction, _raycastDistance);

        if (hit.collider == null)
        {
            _cubeRigidbody.isKinematic = false;
            _cubeRigidbody.AddForce(moveForce, ForceMode2D.Force);
        }
        else
        {
            _cubeRigidbody.isKinematic = true;
            _cubeRigidbody.velocity = Vector2.zero;
        }
    }
}
