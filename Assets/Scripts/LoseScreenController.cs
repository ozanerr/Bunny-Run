using UnityEngine;
using UnityEngine.SceneManagement;

public class LoseScreenController : MonoBehaviour
{
    public float delayBeforeLoad = 5f; 
    void Start()
    {
        Invoke(nameof(GoToLevel1), delayBeforeLoad);
    }

    void GoToLevel1()
    {
        SceneManager.LoadScene(1);
    }
}
