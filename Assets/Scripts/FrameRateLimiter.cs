using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    void Awake()
    {
        // Mengunci game agar berjalan di maksimal 60 FPS
        Application.targetFrameRate = 60;
    }
}