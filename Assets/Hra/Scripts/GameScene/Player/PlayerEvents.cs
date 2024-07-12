using System;

public static class PlayerEvents
{
    public static event Action OnPlayerDeath;
    public static void OnPlayerDeathInvoke()
    {
        OnPlayerDeath?.Invoke();
    }
}
