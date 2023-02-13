using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResumeButton : MonoBehaviour
{
    public void Resume()
    {
        GameManager.Instance.TogglePause();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
