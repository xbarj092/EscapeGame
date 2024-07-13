using UnityEngine;
 
public class MenuCanvasController : MonoBehaviour
{
    // bound from inspector
    public void PlayGame()
    {
        SceneLoadManager.Instance.GoMenuToGame();
    }

    // bound from inspector
    public void ExitGame()
    {
        Application.Quit();
    }
}
