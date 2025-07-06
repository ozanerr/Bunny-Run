using UnityEngine;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    [Header("Tombol & Ikon")]
    public Button pauseButton;
    public Sprite pauseIcon;
    public Sprite resumeIcon;

    void Update()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.isPaused)
            {
                pauseButton.image.sprite = resumeIcon;
            }
            else
            {
                pauseButton.image.sprite = pauseIcon;
            }
        }
    }
}