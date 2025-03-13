using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMANAGER : MonoBehaviour
{

    public GameObject menuscreen;
    public GameObject ingamescreen;
    public GameObject pausescreen;
    public GameObject gameoverscreen;

    public void Resume()
    {
        GameManager.Instance.isGamePaused = false;
        menuscreen.SetActive(false);
        ingamescreen.SetActive(true);
        pausescreen.SetActive(false);
        gameoverscreen.SetActive(false);
    }
    
    public void start()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void retry() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void exit() { 
    
        Application.Quit();
    }

}
