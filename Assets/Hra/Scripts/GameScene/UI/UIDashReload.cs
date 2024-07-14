using UnityEngine;
using UnityEngine.UI;

public class UIDashReload : MonoBehaviour
{
    [SerializeField] private Image _fillImage;

    private CharacterController2D _controller;

    private void Awake()
    {
        GameObject player = GameObject.FindGameObjectWithTag(GlobalConstants.Tags.Player.ToString());
        if (player != null)
        {
            player.TryGetComponent(out _controller);
        }
    }

    private void Update()
    {
        if (_controller.TimeToNextDash() > 0)
        {
            UpdateProgressBar(_controller.TimeToNextDash());
        }
    }

    private void UpdateProgressBar(float timeToNextDash)
    {
        _fillImage.fillAmount = timeToNextDash;
    }
}
