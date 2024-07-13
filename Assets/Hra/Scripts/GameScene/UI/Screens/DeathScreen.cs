using System.Collections;
using UnityEngine;

public class DeathScreen : GameScreen
{
    [SerializeField] private float _timeToLive;

    private void Start()
    {
        StartCoroutine(nameof(LifeTime));
    }

    private IEnumerator LifeTime()
    {
        yield return new WaitForSeconds(_timeToLive);
        SceneLoadManager.Instance.GoGameToMenu();
    }
}
