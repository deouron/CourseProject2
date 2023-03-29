using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMenu : MonoBehaviour
{
    public void LoadMain() {
        Debug.Log("Load main");
        PauseMenu.isPause = false;
        SceneManager.LoadScene(0); // name of scene
    }
}
