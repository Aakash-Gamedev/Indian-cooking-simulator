using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; } // Singleton instance

    public int money;
    public int Startlives;
    public int lives;
    public bool isGamePaused;
    public bool gameover;

    public GameObject pauseScreen;
    public GameObject gameoverScreen;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI lifeText;

    private void Start()
    {
        moneyText.text = "MONEY - " + money;
        lifeText.text = "LIVES - " + lives;
    }

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes (optional)
        }
        else
        {
            Destroy(gameObject); // Ensure only one instance exists
            return; // Important: Exit Awake() to prevent further initialization
        }

        // Initialize game variables here
        money = 0;
        lives = Startlives; // Start with 3 lives (or your desired starting amount)
        isGamePaused = false;
    }

    private void Update()
    {
        if (!isGamePaused)
        {
            Time.timeScale = 1f; // Restore game time
            pauseScreen.SetActive(false);
        }
        else {
            Time.timeScale = 0f; // Restore game time
            pauseScreen.SetActive(true);
        }

        // Toggle pause with Escape key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    // Example game functions
    public void AddMoney(int amount)
    {
        money += amount;
        moneyText.text = "MONEY - " + money;
    }

    public void RemoveLife()
    {
        lives--;
        Debug.Log("Lives remaining: " + lives); // For debugging
        lifeText.text = "LIVES - " + lives;
        if (lives <= 0)
        {
            GameOver();
        }
    }


    public void TogglePause()
    {
        isGamePaused = !isGamePaused; // Toggle the pause state

        if (isGamePaused)
        {
            Time.timeScale = 0f; // Freeze game time
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1f; // Restore game time
        }
    }

    private void GameOver()
    {

        gameoverScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
}