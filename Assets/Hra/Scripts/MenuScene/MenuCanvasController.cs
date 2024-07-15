using System.Collections;
using UnityEngine;
using UnityEngine.UI;
 
public class MenuCanvasController : MonoBehaviour
{
    [SerializeField] private Button _continueButton;

    private void OnEnable()
    {
        if (GameManager.Instance.CurrentLevel == 0)
        {
            _continueButton.interactable = false;
        }
    }

    // bound from inspector
    public void PlayGame()
    {
        GameManager.Instance.CurrentLevel = 0;
        SceneLoadManager.Instance.GoMenuToGame();
    }

    // bound from inspector
    public void Continue()
    {
        GameManager.Instance.Respawn();
        SceneLoadManager.Instance.GoMenuToGame();
    }

    // bound from inspector
    public void ExitGame()
    {
        Application.Quit();
    }
}
