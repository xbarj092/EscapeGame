using System.Collections;
using UnityEngine;

public class TutorialCubesAction : TutorialAction
{
    [Header("Colliders")]
    [SerializeField] private TutorialCollision _nearMoveableCollider;
    [SerializeField] private TutorialCollision _movedCubeCollider;
    [SerializeField] private TutorialCollision _nearBreakableCollider;
    [SerializeField] private TutorialCollision _brokenCubeCollider;

    [Header("TextPositions")]
    [SerializeField] private Transform _nearMoveableTransform;
    [SerializeField] private Transform _nearBreakableTransform;

    public override void StartAction()
    {
        _nearMoveableCollider.OnTriggerEntered += OnNearMoveableTutorial;
    }

    private void OnNearMoveableTutorial()
    {
        _nearMoveableCollider.OnTriggerEntered -= OnNearMoveableTutorial;
        _tutorialPlayer.SetTextPosition(_nearMoveableTransform.localPosition);
        _tutorialPlayer.MoveToNextNarratorText();
        _movedCubeCollider.OnTriggerEntered += OnCubeMoved;
    }

    private void OnCubeMoved()
    {
        _movedCubeCollider.OnTriggerEntered -= OnCubeMoved;
        _tutorialPlayer.TextFadeAway();
        _nearBreakableCollider.OnTriggerEntered += OnNearBreakableTutorial;
    }

    private void OnNearBreakableTutorial()
    {
        _nearBreakableCollider.OnTriggerEntered -= OnNearBreakableTutorial;
        _tutorialPlayer.SetTextPosition(_nearBreakableTransform.localPosition);
        _tutorialPlayer.MoveToNextNarratorText();
        _brokenCubeCollider.OnTriggerEntered += OnCubeBroken;
    }

    private void OnCubeBroken()
    {
        _brokenCubeCollider.OnTriggerEntered += OnCubeBroken;
        StartCoroutine(OnCubeBrokenCoroutine());
    }

    private IEnumerator OnCubeBrokenCoroutine()
    {
        _tutorialPlayer.TextFadeAway();
        yield return new WaitForSeconds(0.3f);
        OnActionFinishedInvoke();
    }

    public override void Exit()
    {
    }
}
