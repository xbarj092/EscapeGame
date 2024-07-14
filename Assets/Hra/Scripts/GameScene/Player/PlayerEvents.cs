using System;

public static class PlayerEvents
{
    public static event Action OnPlayerDeath;
    public static void OnPlayerDeathInvoke()
    {
        OnPlayerDeath?.Invoke();
    }

    public static event Action OnPlayerMoved;
    public static void OnPlayerMovedInvoke()
    {
        OnPlayerMoved?.Invoke();
    }

    public static event Action OnPlayerJumped;
    public static void OnPlayerJumpedInvoke()
    {
        OnPlayerJumped?.Invoke();
    }

    public static event Action OnPlayerDoubleJumped;
    public static void OnPlayerDoubleJumpedInvoke()
    {
        OnPlayerDoubleJumped?.Invoke();
    }

    public static event Action OnPlayerClimbed;
    public static void OnPlayerClimbedInvoke()
    {
        OnPlayerClimbed?.Invoke();
    }

    public static event Action OnPlayerDashed;
    public static void OnPlayerDashedInvoke()
    {
        OnPlayerDashed?.Invoke();
    }
}
