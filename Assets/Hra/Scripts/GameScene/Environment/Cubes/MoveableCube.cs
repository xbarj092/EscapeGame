using System.Collections;
using UnityEngine;

public class MoveableCube : BaseCube
{
    [SerializeField] private float _moveDistance = 1f;
    [SerializeField] private float _moveDuration = 0.5f;

    protected override void HandleAction()
    {
        StartCoroutine(MoveCube());
    }

    private IEnumerator MoveCube()
    {
        Vector3 startPosition = transform.position;
        Vector3 direction = _rigidbody.velocity.x > 0 ? Vector3.right : Vector3.left;
        Vector3 endPosition = transform.position + direction * _moveDistance;
        float elapsedTime = 0f;

        while (elapsedTime < _moveDuration)
        {
            transform.position = Vector3.Lerp(startPosition, endPosition, elapsedTime / _moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = endPosition;
    }
}
