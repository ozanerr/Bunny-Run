using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScreenController : MonoBehaviour
{
    public float delayBeforeLoad = 5f; 
    void Start()
    {
        Invoke(nameof(GoToCreditScene), delayBeforeLoad);
    }

    void GoToCreditScene()
    {
      SceneManager.LoadScene("CreditScene");
    }
}
