using System.Collections;
using System.Threading;
using UnityEngine;

public enum WatcherState
{
    Patrol = 0,
    Aggro = 1
}

public class Watcher : MonoBehaviour
{
    [SerializeField] private Watchlight _watchlight;
    [SerializeField] private Transform _eye;
    [SerializeField] private Transform _startTransform;
    [SerializeField] private Transform _endTransform;

    [Space(5)]
    [SerializeField] private float _speed;
    [SerializeField] private float _watchAngleThreshold;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _rotationTime;

    private WatcherState _currentState;
    private Transform _playerTransform;

    private Quaternion _targetRotation;
    private Vector3 _targetPosition;
    private bool _isWatching = false;
    private Quaternion _defaultEyeRotation;

    private Quaternion _rotationToEnd;
    private Quaternion _rotationToStart;

    private const float TARGET_POSITION_THRESHOLD = 0.01f;

    private CancellationTokenSource _cancellationTokenSource;

    private void OnEnable()
    {
        _watchlight.OnPlayerSpotted += OnPlayerSpotted;
        _watchlight.OnPlayerTargetLost += OnPlayerLost;
    }

    private void OnDisable()
    {
        _watchlight.OnPlayerSpotted -= OnPlayerSpotted;
        _watchlight.OnPlayerTargetLost -= OnPlayerLost;
    }

    private void Start()
    {
        _currentState = WatcherState.Patrol;
        _rotationToStart = transform.rotation;
        _rotationToEnd = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + 100);

        transform.SetPositionAndRotation(_startTransform.position, _rotationToEnd);
        _targetPosition = _endTransform.position;
        _defaultEyeRotation = _eye.localRotation;

        _cancellationTokenSource = new CancellationTokenSource();
    }

    private void FixedUpdate()
    {
        if (_currentState == WatcherState.Aggro)
        {
            ExecuteAggroBehavior();
        }
        else if (!_isWatching)
        {
            HandleMovement();
        }
    }

    private void HandleMovement()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, _speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, _targetPosition) < TARGET_POSITION_THRESHOLD)
        {
            StartWatching();
        }
    }

    private void StartWatching()
    {
        if (_cancellationTokenSource != null)
        {
            _cancellationTokenSource.Cancel();
        }

        _cancellationTokenSource = new CancellationTokenSource();
        StartCoroutine(DelayedWatch(_cancellationTokenSource.Token));
    }

    private IEnumerator DelayedWatch(CancellationToken token)
    {
        float watchDuration = Random.Range(0, 4);
        _isWatching = true;

        _targetRotation = _targetPosition == _startTransform.position ? _rotationToEnd : _rotationToStart;

        if (watchDuration > 0.5f)
        {
            float elapsed = 0f;
            Quaternion startRotation = _eye.localRotation;

            while (elapsed < watchDuration)
            {
                if (token.IsCancellationRequested)
                {
                    yield break;
                }

                Quaternion randomRotation = _defaultEyeRotation * Quaternion.Euler(0, 0, Random.Range(-_watchAngleThreshold, _watchAngleThreshold));
                float transitionDuration = 0.5f;
                float time = 0f;

                while (time < transitionDuration && elapsed < watchDuration)
                {
                    if (token.IsCancellationRequested)
                    {
                        yield break;
                    }

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

        _targetRotation = _targetPosition == _startTransform.position ? _rotationToEnd : _rotationToStart;
        StartCoroutine(SwitchTarget(_cancellationTokenSource.Token));
    }

    private void OnPlayerSpotted(Transform playerTransform)
    {
        _playerTransform = playerTransform;
        _currentState = WatcherState.Aggro;
        _cancellationTokenSource.Cancel();
    }

    private void ExecuteAggroBehavior()
    {
        if (_playerTransform != null)
        {
            Vector3 directionToPlayer = _playerTransform.position - transform.position;
            float targetAngle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;

            float currentAngle = transform.rotation.eulerAngles.z;
            float eyeCurrentAngle = _eye.rotation.eulerAngles.z;

            float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);
            float eyeAngleDifference = Mathf.DeltaAngle(eyeCurrentAngle, targetAngle);

            float angleThreshold = 1f;

            if (Mathf.Abs(angleDifference) > angleThreshold || Mathf.Abs(eyeAngleDifference) > angleThreshold)
            {
                Quaternion watcherLookRotation = Quaternion.Euler(0, 0, targetAngle + 180);
                Quaternion eyeLookRotation = Quaternion.Euler(0, 0, targetAngle + 90);
                transform.rotation = Quaternion.Slerp(transform.rotation, watcherLookRotation, Time.deltaTime * 2);
                _eye.rotation = Quaternion.Slerp(_eye.rotation, eyeLookRotation, Time.deltaTime);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0, targetAngle);
                _eye.rotation = Quaternion.Euler(0, 0, targetAngle);
            }
        }
    }

    private void OnPlayerLost()
    {
        _playerTransform = null;
        _currentState = WatcherState.Patrol;
        _cancellationTokenSource = new CancellationTokenSource();
        _watchlight.SetAlert(true);

        StartCoroutine(SwitchTarget(_cancellationTokenSource.Token));
    }

    private IEnumerator SwitchTarget(CancellationToken token)
    {
        Debug.Log(transform.rotation.z);
        float rotationTime = 0f;
        while (rotationTime < _rotationTime)
        {
            if (token.IsCancellationRequested)
            {
                yield break;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, _targetRotation, rotationTime);
            _eye.localRotation = Quaternion.Slerp(_eye.localRotation, _defaultEyeRotation, rotationTime);
            rotationTime += Time.deltaTime * _rotationSpeed;
            yield return null;
        }

        _targetPosition = _targetPosition == _startTransform.position ? _endTransform.position : _startTransform.position;
        _isWatching = false;
    }
}
