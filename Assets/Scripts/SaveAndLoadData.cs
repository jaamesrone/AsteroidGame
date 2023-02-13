using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveAndLoadData : MonoBehaviour
{
 
    public void SaveGame()
    {
        GameManager.Instance.Save();
    }

    public void LoadGame()
    {
        GameManager.Instance.LoadGame();
    }
}