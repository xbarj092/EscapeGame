using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    [SerializeField] private List<Vector3> _playerStartPositions = new();

    private CharacterController2D _controller;
    private GameCanvasController _gameCanvasController;
    private CameraManager _cameraManager;

    public int CurrentLevel = 0;

    public bool CanJump = false;
    public bool CanDoubleJump = false;
    public bool CanDash = false;

    public void Respawn()
    {
        StartCoroutine(RestartCoroutine());
    }

    private IEnumerator RestartCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(MoveToLevel(CurrentLevel));
    }

    public IEnumerator Final()
    {
        _controller = FindObjectOfType<CharacterController2D>();
        _gameCanvasController = FindObjectOfType<GameCanvasController>();
        _cameraManager = FindObjectOfType<CameraManager>();

        _controller.CanMove = false;
        BlackOutScreen(true);
        yield return new WaitForSeconds(1);
        SetCorrectCamera(4);
        yield return new WaitForSeconds(2);
        BlackOutScreen(false);
        yield return new WaitForSeconds(1);
        yield return StartCoroutine(FindObjectOfType<FinalElevator>().GoUp());
        WhiteOutScreen();
        yield return new WaitForSeconds(3);
        SceneLoadManager.Instance.GoGameToMenu();
    }

    public IEnumerator MoveToLevel(int levelToLoad)
    {
        _controller = FindObjectOfType<CharacterController2D>();
        _gameCanvasController = FindObjectOfType<GameCanvasController>();
        _cameraManager = FindObjectOfType<CameraManager>();

        _controller.CanMove = false;
        BlackOutScreen(true);
        yield return new WaitForSeconds(1);
        SetPlayerTransform(levelToLoad);
        SetCorrectCamera(levelToLoad);
        yield return new WaitForSeconds(2);
        BlackOutScreen(false);
        yield return new WaitForSeconds(5);
        CurrentLevel = levelToLoad;
        _controller.CanMove = true;
    }

    private void BlackOutScreen(bool black)
    {
        StartCoroutine(_gameCanvasController.BlackOutScreen(black));
    }

    private void WhiteOutScreen()
    {
        StartCoroutine(_gameCanvasController.WhiteOutScreen());
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
