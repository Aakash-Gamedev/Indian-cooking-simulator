using UnityEngine;

public class FrameRateController : MonoBehaviour
{

    // Public Variables
    [Header("Settings")]
    public int targetFrameRate = 60;
    public bool enableVSync = true; 

    void Start()
    {
        // Set the target frame rate
        Application.targetFrameRate = targetFrameRate;

        // Enable or disable VSync
        if (enableVSync)
        {
            QualitySettings.vSyncCount = 1; // 1 frame interval
        }
        else
        {
            QualitySettings.vSyncCount = 0; // VSync off
        }
    }

    // Optional: Method to dynamically set frame rate
    public void SetFrameRate(int newFrameRate)
    {
        targetFrameRate = newFrameRate;
        Application.targetFrameRate = targetFrameRate;
    }

    // Optional: Method to dynamically enable or disable VSync
    public void SetVSync(bool isEnabled)
    {
        enableVSync = isEnabled;
        QualitySettings.vSyncCount = enableVSync ? 1 : 0;
    }
}