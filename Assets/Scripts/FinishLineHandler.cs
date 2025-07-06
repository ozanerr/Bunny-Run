using UnityEngine;

public class FinishLineHandler : MonoBehaviour
{
    private bool hasBeenTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (hasBeenTriggered) return;

        if (other.CompareTag("Runner"))
        {
            Debug.Log("Finish Line Reached! Starting level transition...");
            AudioManager.instance.StopMusic();

            AudioManager.instance.PlayFinishSoundAndGoToNextLevel();

            hasBeenTriggered = true;
        }
    }
}