using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SplashManager : MonoBehaviour
{
    [Header("UI References")]
    public Image loadingBarFill;
    public TextMeshProUGUI loadingText;
    public TextMeshProUGUI gameTitle;
    
    [Header("Settings")]
    public float minimumSplashTime = 3f;
    public float fadeInTime = 1f;
    public float fadeOutTime = 1f;
    public string mainSceneName = "level1";
    
    private void Start()
    {
        StartCoroutine(SplashSequence());
    }
    
    private IEnumerator SplashSequence()
    {
        
        
        yield return StartCoroutine(SimulateLoading());
        
        yield return new WaitForSeconds(minimumSplashTime);
        
        
        SceneManager.LoadScene(mainSceneName);
    }
    
    private IEnumerator SimulateLoading()
    {
        float progress = 0f;
        string[] loadingMessages = {
            "Loading...",
            "Preparing game...",
            "Loading assets...",
            "Almost ready...",
            "Loading complete!"
        };
        
        while (progress < 1f)
        {
            progress += Random.Range(0.05f, 0.15f);
            progress = Mathf.Clamp01(progress);
            
            loadingBarFill.fillAmount = progress;
            
            int messageIndex = Mathf.FloorToInt(progress * (loadingMessages.Length - 1));
            messageIndex = Mathf.Clamp(messageIndex, 0, loadingMessages.Length - 1);
            loadingText.text = loadingMessages[messageIndex];
            
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
        
        loadingBarFill.fillAmount = 1f;
        loadingText.text = "Loading complete!";
        yield return new WaitForSeconds(0.5f);
    }
}