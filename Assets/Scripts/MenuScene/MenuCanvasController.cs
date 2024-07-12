using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class MenuCanvasController : MonoBehaviour
{
    // bound from inspector
    public void PlayGame()
    {
        SceneLoadManager.Instance.GoMenuToGame();
    }
}
