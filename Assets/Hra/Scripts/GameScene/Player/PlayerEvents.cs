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

    public static event Action OnPlayerClimbed;
    public static void OnPlayerClimbedInvoke()
    {
        OnPlayerClimbed?.Invoke();
    }
}
