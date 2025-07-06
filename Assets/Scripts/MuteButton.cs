using UnityEngine;
using UnityEngine.UI;

public class MuteButton : MonoBehaviour
{
    public AudioManager audioManager;
    public Sprite soundOnIcon;
    public Sprite soundOffIcon;
    private Image buttonImage;
    private bool isMuted = false;

    void Start()
    {
        buttonImage = GetComponent<Image>();
        UpdateButtonState();
    }

    public void ToggleMute()
    {
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not assigned to MuteButton!");
            return;
        }

        isMuted = !isMuted;

        if (isMuted)
        {
            audioManager.bgmSource.volume = 0f;
        }
        else
        {
            audioManager.bgmSource.volume = 1f;
        }

        UpdateButtonState();
    }

    private void UpdateButtonState()
    {
        if (buttonImage == null) return;

        if (isMuted)
        {
            buttonImage.sprite = soundOffIcon;
        }
        else
        {
            buttonImage.sprite = soundOnIcon;
        }
    }
}