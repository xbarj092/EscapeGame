using Cinemachine;
using System.Collections;
using UnityEngine;

public class TutorialMovementAction : TutorialAction
{
    [Header("Colliders")]
    [SerializeField] private TutorialCollision _jumpCollider;
    [SerializeField] private TutorialCollision _jumpMadeCollider;
    [SerializeField] private TutorialCollision _doubleJumpCollider;
    [SerializeField] private TutorialCollision _doubleJumpMadeCollider;
    [SerializeField] private TutorialCollision _climbCollider;
    [SerializeField] private TutorialCollision _dashCollider;
    [SerializeField] private TutorialCollision _dashMadeCollider;
    [SerializeField] private TutorialCollision _nextTutorialCollider;

    [Header("TextPositions")]
    [SerializeField] private Transform _moveTransform;
    [SerializeField] private Transform _jumpTransform;
    [SerializeField] private Transform _doubleJumpTransform;
    [SerializeField] private Transform _climbTransform;
    [SerializeField] private Transform _dashTransform;
    [SerializeField] private Transform _afterDashTransform;

    private CharacterController2D _controller;
    private UIDashReload _dashReload;

    public override void StartAction()
    {
        _controller = FindObjectOfType<CharacterController2D>();
        _dashReload = FindObjectOfType<UIDashReload>();

        _tutorialPlayer.MoveToNextNarratorText();
        _tutorialPlayer.SetTextPosition(_moveTransform.localPosition);
        PlayerEvents.OnPlayerMoved += OnPlayerMoved;
    }

    private void OnPlayerMoved()
    {
        PlayerEvents.OnPlayerMoved -= OnPlayerMoved;
        _tutorialPlayer.TextFadeAway();
        _jumpCollider.OnTriggerEntered += OnNearJumpTutorial;
    }

    private void OnNearJumpTutorial()
    {
        _jumpCollider.OnTriggerEntered -= OnNearJumpTutorial;
        _controller.CanJump = true;
        _tutorialPlayer.MoveToNextNarratorText();
        _tutorialPlayer.SetTextPosition(_jumpTransform.localPosition);
        _jumpMadeCollider.OnTriggerEntered += OnJumpMadeTutorial;
    }

    private void OnJumpMadeTutorial()
    {
        _jumpMadeCollider.OnTriggerEntered -= OnJumpMadeTutorial;
        _tutorialPlayer.TextFadeAway();
        _doubleJumpCollider.OnTriggerEntered += OnNearDoubleJumpTutorial;
    }

    private void OnNearDoubleJumpTutorial()
    {
        _doubleJumpCollider.OnTriggerEntered -= OnNearDoubleJumpTutorial;
        _controller.CanDoubleJump = true;
        _tutorialPlayer.MoveToNextNarratorText();
        _tutorialPlayer.SetTextPosition(_doubleJumpTransform.localPosition);
        _doubleJumpMadeCollider.OnTriggerEntered += OnPlayerDoubleJumpMade;
    }

    private void OnPlayerDoubleJumpMade()
    {
        _doubleJumpMadeCollider.OnTriggerEntered -= OnPlayerDoubleJumpMade;
        _tutorialPlayer.TextFadeAway();
        _climbCollider.OnTriggerEntered += OnNearClimbTutorial;
    }

    private void OnNearClimbTutorial()
    {
        _climbCollider.OnTriggerEntered -= OnNearClimbTutorial;
        _tutorialPlayer.MoveToNextNarratorText();
        _tutorialPlayer.SetTextPosition(_climbTransform.localPosition);
        _dashCollider.OnTriggerEntered += OnPlayerNearDashTutorial;
    }

    private void OnPlayerNearDashTutorial()
    {
        _dashCollider.OnTriggerEntered -= OnPlayerNearDashTutorial;
        StartCoroutine(OnPlayerNearDashTutorialCoroutine());
    }

    private IEnumerator OnPlayerNearDashTutorialCoroutine()
    {
        _tutorialPlayer.TextFadeAway();
        yield return new WaitForSeconds(0.3f);
        _controller.CanDash = true;
        _tutorialPlayer.MoveToNextNarratorText();
        _tutorialPlayer.SetTextPosition(_dashTransform.localPosition);
        _dashMadeCollider.OnTriggerEntered += OnPlayerDashed;
    }

    private void OnPlayerDashed()
    {
        _dashMadeCollider.OnTriggerEntered -= OnPlayerDashed;
        StartCoroutine(OnPlayerDashedCoroutine());
    }

    private IEnumerator OnPlayerDashedCoroutine()
    {
        _tutorialPlayer.TextFadeAway();
        _nextTutorialCollider.OnTriggerEntered += OnNextTutorialTriggered;
        yield return new WaitForSeconds(0.3f);
        _dashReload.gameObject.SetActive(true);
        _tutorialPlayer.MoveToNextNarratorText();
        _tutorialPlayer.SetTextPosition(_afterDashTransform.localPosition);
    }

    private void OnNextTutorialTriggered()
    {
        _nextTutorialCollider.OnTriggerEntered -= OnNextTutorialTriggered;
        OnActionFinishedInvoke();
        MoveToNextTutorial();
    }

    private void MoveToNextTutorial()
    {
        TutorialManager.Instance.InstantiateTutorial(TutorialID.Cubes);
        CinemachineVirtualCamera camera = FindObjectOfType<CinemachineVirtualCamera>();
        if (camera != null)
        {
            camera.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition = 1;
        }
    }

    public override void Exit()
    {
    }
}
