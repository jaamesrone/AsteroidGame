using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class LoadGame : MonoBehaviour
{
    public Button playButton;
    public Button ControlButton;
    public Button GoBackButton;
    private bool isDead = false;

    private void Start()
    {
        playButton.onClick.AddListener(LoadAsteroidGame);
        ControlButton.onClick.AddListener(GoToControls);
    }


    public void LoadAsteroidGame()
    {
        
        SceneManager.LoadScene(4);
  
    }

    public void GoBack()
    {
        SceneManager.LoadScene(0);
        GameManager.Instance.Reset();
    }

    public void GoToControls()
    {
        SceneManager.LoadScene(1);
    }
}
