using System.Collections;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private bool _shouldClose;
    [SerializeField] private int _levelToLoad;

    [SerializeField] private Transform _goingUp;
    [SerializeField] private Transform _goingUpStartTransform;
    [SerializeField] private Transform _goingUpEndTransform;

    [SerializeField] private Transform _leftDoor;
    [SerializeField] private Transform _leftDoorStartTransform;
    [SerializeField] private Transform _leftDoorEndTransform;

    [SerializeField] private Transform _rightDoor;
    [SerializeField] private Transform _rightDoorStartTransform;
    [SerializeField] private Transform _rightDoorEndTransform;

    [SerializeField] private float _doorClosingDuration = 1f;
    [SerializeField] private float _goingUpDuration = 2f;

    private CharacterController2D _controller;

    private void Awake()
    {
        _controller = FindObjectOfType<CharacterController2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_shouldClose && collision.gameObject.CompareTag(GlobalConstants.Tags.Player.ToString()))
        {
            StartCoroutine(CloseElevator());
        }
    }

    private IEnumerator CloseElevator()
    {
        yield return StartCoroutine(CloseDoors());
        StartCoroutine(GoUp());
        yield return StartCoroutine(GameManager.Instance.MoveToLevel(_levelToLoad));
    }

    private IEnumerator CloseDoors()
    {
        float elapsedTime = 0f;

        Vector3 leftDoorStartPos = _leftDoorStartTransform.position;
        Vector3 leftDoorEndPos = _leftDoorEndTransform.position;

        Vector3 leftDoorStartScale = _leftDoorStartTransform.localScale;
        Vector3 leftDoorEndScale = _leftDoorEndTransform.localScale;

        Vector3 rightDoorStartPos = _rightDoorStartTransform.position;
        Vector3 rightDoorEndPos = _rightDoorEndTransform.position;

        Vector3 rightDoorStartScale = _rightDoorStartTransform.localScale;
        Vector3 rightDoorEndScale = _rightDoorEndTransform.localScale;

        while (elapsedTime < _doorClosingDuration)
        {
            float t = elapsedTime / _doorClosingDuration;
            _leftDoor.position = Vector3.Lerp(leftDoorStartPos, leftDoorEndPos, t);
            _rightDoor.position = Vector3.Lerp(rightDoorStartPos, rightDoorEndPos, t);
            _leftDoor.localScale = Vector3.Lerp(leftDoorStartScale, leftDoorEndScale, t);
            _rightDoor.localScale = Vector3.Lerp(rightDoorStartScale, rightDoorEndScale, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _leftDoor.position = leftDoorEndPos;
        _rightDoor.position = rightDoorEndPos;
        _leftDoor.localScale = leftDoorEndScale;
        _rightDoor.localScale = rightDoorEndScale;
    }

    private IEnumerator GoUp()
    {
        float elapsedTime = 0f;

        Vector3 startPos = _goingUpStartTransform.position;
        Vector3 endPos = _goingUpEndTransform.position;

        while (elapsedTime < _goingUpDuration)
        {
            float t = elapsedTime / _goingUpDuration;
            _goingUp.position = Vector3.Lerp(startPos, endPos, t);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _goingUp.position = endPos;
    }
}
