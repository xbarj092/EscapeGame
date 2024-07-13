using System.Collections;
using UnityEngine;

public class Watcher : MonoBehaviour
{
    [SerializeField] private Transform _eye;
    [SerializeField] private Transform _startTransform;
    [SerializeField] private Transform _endTransform;
    [SerializeField] private float _speed;
    [SerializeField] private float _watchAngleThreshold;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _rotationTime;

    private Vector3 _targetPosition;
    private bool _isWatching = false;
    private Quaternion _defaultEyeRotation;

    private Quaternion _rotationToEnd;
    private Quaternion _rotationToStart;

    private const float TARGET_POSITION_THRESHOLD = 0.01f;

    private void Start()
    {
        _rotationToStart = transform.rotation;
        _rotationToEnd = Quaternion.Euler(transform.rotation.x, transform.rotation.y, transform.rotation.z + 100);

        transform.SetPositionAndRotation(_startTransform.position, _rotationToEnd);
        _targetPosition = _endTransform.position;
        _defaultEyeRotation = _eye.localRotation;
    }

    private void Update()
    {
        if (!_isWatching)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _targetPosition) < TARGET_POSITION_THRESHOLD)
        {
            StartCoroutine(nameof(DelayedWatch));
        }
    }

    private IEnumerator DelayedWatch()
    {
        float watchDuration = Random.Range(0, 4);
        _isWatching = true;

        if (watchDuration > 0.5f)
        {
            float elapsed = 0f;
            Quaternion startRotation = _eye.localRotation;

            while (elapsed < watchDuration)
            {
                Quaternion randomRotation = _defaultEyeRotation * Quaternion.Euler(0, 0, Random.Range(-_watchAngleThreshold, _watchAngleThreshold));
                float transitionDuration = 0.5f;
                float time = 0f;

                while (time < transitionDuration && elapsed < watchDuration)
                {
                    _eye.localRotation = Quaternion.Slerp(startRotation, randomRotation, time / transitionDuration);
                    time += Time.deltaTime;
                    elapsed += Time.deltaTime;
                    yield return null;
                }

                startRotation = randomRotation;
            }
        }
        else
        {
            yield return new WaitForSeconds(watchDuration);
        }

        Quaternion targetRotation = _targetPosition == _startTransform.position ? _rotationToEnd : _rotationToStart;
        float rotationTime = 0f;

        while (rotationTime < _rotationTime)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationTime);
            _eye.localRotation = Quaternion.Slerp(_eye.localRotation, _defaultEyeRotation, rotationTime);
            rotationTime += Time.deltaTime * _rotationSpeed;
            yield return null;
        }

        _targetPosition = _targetPosition == _startTransform.position ? _endTransform.position : _startTransform.position;
        _isWatching = false;
    }
}
