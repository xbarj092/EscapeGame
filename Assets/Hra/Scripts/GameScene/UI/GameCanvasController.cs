using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCanvasController : MonoBehaviour
{
    [SerializeField] private DeathScreen _deathScreenPrefab;

    public SpriteRenderer Background;
    public float fadeDuration = 1f;

    private Dictionary<GameScreenType, GameScreen> _instantiatedScreens = new();

    private void OnEnable()
    {
        PlayerEvents.OnPlayerDeath += ShowDeathScreen;
    }

    private void OnDisable()
    {
        PlayerEvents.OnPlayerDeath -= ShowDeathScreen;
    }
    
    private void ShowDeathScreen()
    {
        ShowGameScreen(GameScreenType.Death);
    }

    private void ShowGameScreen(GameScreenType gameScreenType)
    {
        if ((_instantiatedScreens.ContainsKey(gameScreenType) && _instantiatedScreens[gameScreenType] == null) || 
            !_instantiatedScreens.ContainsKey(gameScreenType))
        {
            InstantiateScreen(gameScreenType);
        }

        _instantiatedScreens[gameScreenType].Open();
    }

    private void CloseGameScreen(GameScreenType gameScreenType)
    {
        if (_instantiatedScreens.ContainsKey(gameScreenType))
        {
            ScreenManager.Instance.SetActiveGameScreen(GetActiveGameScreen(gameScreenType));
            _instantiatedScreens[gameScreenType].Close();
            _instantiatedScreens.Remove(gameScreenType);
        }
    }

    private void InstantiateScreen(GameScreenType gameScreenType)
    {
        GameScreen screenInstance = GetRelevantScreen(gameScreenType);
        if (screenInstance != null)
        {
            _instantiatedScreens[gameScreenType] = screenInstance;
            ScreenManager.Instance.SetActiveGameScreen(screenInstance);
        }
    }

    private GameScreen GetRelevantScreen(GameScreenType gameScreenType)
    {
        return gameScreenType switch
        {
            GameScreenType.Death => Instantiate(_deathScreenPrefab, transform),
            _ => null
        };
    }

    private GameScreen GetActiveGameScreen(GameScreenType gameScreenType)
    {
        return gameScreenType switch
        {
            _ => null
        };
    }

    public IEnumerator BlackOutScreen(bool black)
    {
        float startAlpha = Background.color.a;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            timeElapsed += Time.deltaTime;
            Background.color = new(Background.color.r, Background.color.g, Background.color.b, Mathf.Lerp(startAlpha, black ? 1 : 0, timeElapsed / fadeDuration));
            yield return null;
        }

        Background.color = new(Background.color.r, Background.color.g, Background.color.b, black ? 1 : 0);
    }
}
