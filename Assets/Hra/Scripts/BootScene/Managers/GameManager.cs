using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private List<Vector3> _playerStartPositions = new();

    private CharacterController2D _controller;
    private GameCanvasController _gameCanvasController;
    private CameraManager _cameraManager;

    public IEnumerator MoveToLevel(int levelToLoad)
    {
        _controller = FindObjectOfType<CharacterController2D>();
        _gameCanvasController = FindObjectOfType<GameCanvasController>();
        _cameraManager = FindObjectOfType<CameraManager>();

        BlackOutScreen(true);
        yield return new WaitForSeconds(1);
        SetPlayerTransform(levelToLoad);
        SetCorrectCamera(levelToLoad);
        yield return new WaitForSeconds(1);
        BlackOutScreen(false);
        yield return new WaitForSeconds(5);
    }

    private void BlackOutScreen(bool black)
    {
        StartCoroutine(_gameCanvasController.BlackOutScreen(black));
    }

    private void SetPlayerTransform(int levelToLoad)
    {
        _controller.transform.localPosition = _playerStartPositions[levelToLoad - 1];
    }

    private void SetCorrectCamera(int levelToLoad)
    {
        _cameraManager.EnterLevel(levelToLoad);
    }
}
