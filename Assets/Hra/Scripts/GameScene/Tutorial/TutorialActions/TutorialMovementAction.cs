public class TutorialMovementAction : TutorialAction
{
    public override void StartAction()
    {
        _tutorialPlayer.MoveToNextNarratorText();
        PlayerEvents.OnPlayerMoved += OnPlayerMoved;
    }

    private void OnPlayerMoved()
    {
        PlayerEvents.OnPlayerMoved -= OnPlayerMoved;
        _tutorialPlayer.MoveToNextNarratorText();
        PlayerEvents.OnPlayerJumped += OnPlayerJumped;
    }

    private void OnPlayerJumped()
    {
        PlayerEvents.OnPlayerJumped -= OnPlayerJumped;
        _tutorialPlayer.MoveToNextNarratorText();
        PlayerEvents.OnPlayerDoubleJumped += OnPlayerDoubleJumped;
    }

    private void OnPlayerDoubleJumped()
    {
        PlayerEvents.OnPlayerDoubleJumped -= OnPlayerDoubleJumped;
        _tutorialPlayer.MoveToNextNarratorText();
        PlayerEvents.OnPlayerDashed += OnPlayerDashed;
    }

    private void OnPlayerDashed()
    {
        PlayerEvents.OnPlayerDashed -= OnPlayerDashed;
        OnActionFinishedInvoke();
    }

    public override void Exit()
    {
    }
}
