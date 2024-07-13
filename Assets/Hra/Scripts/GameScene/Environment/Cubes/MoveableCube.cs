using UnityEngine;

public class MoveableCube : BaseCube
{
    [SerializeField] private Rigidbody2D _cubeRigidbody;
    [SerializeField] private Vector2 _force;

    protected override void HandleAction()
    {
        Vector2 moveForce = new(_rigidbody.velocity.x > 0 ? _force.x : -_force.x, 0);
        _cubeRigidbody.AddForce(moveForce, ForceMode2D.Force);
    }
}
