using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    private AudioSource sfxSource;
    public AudioSource bgmSource;

    [Header("BGM")]
    public AudioClip backgroundMusic;

    [Header("Sound Effects")]
    public AudioClip collisionSound;
    public AudioClip startSound;
    public AudioClip gateSound;
    public AudioClip finishSound;
    public AudioClip gameOverSound;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        AudioSource[] sources = GetComponents<AudioSource>();
        sfxSource = sources[0];
        bgmSource = sources[1];

        if (bgmSource != null && backgroundMusic != null)
        {
            bgmSource.clip = backgroundMusic;
        }
    }

    public void PlayMusic()
    {
        if (bgmSource != null && !bgmSource.isPlaying)
        {
            bgmSource.Play();
        }
    }

    public void StopMusic()
    {
        if (bgmSource != null && bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
    }

    public void PlayCollisionSound()
    {
        if (collisionSound != null) sfxSource.PlayOneShot(collisionSound);
    }

    public void PlayStartSound()
    {
        if (startSound != null) sfxSource.PlayOneShot(startSound);
    }

    public void PlayGateSound()
    {
        if (gateSound != null) sfxSource.PlayOneShot(gateSound);
    }

    public void PlayGameOverSound()
    {
        if (gameOverSound != null) sfxSource.PlayOneShot(gameOverSound);
    }

    public void PlayFinishSoundAndGoToNextLevel()
    {
        if (finishSound != null)
        {
            StartCoroutine(PlayAndLoadRoutine());
        }
    }

    private IEnumerator PlayAndLoadRoutine()
    {
        sfxSource.PlayOneShot(finishSound);
        yield return new WaitForSeconds(finishSound.length);
        if (GameManager.instance != null)
        {
            GameManager.instance.LoadNextLevel();
        }
    }
}
