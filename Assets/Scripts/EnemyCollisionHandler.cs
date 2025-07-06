using UnityEngine;

public class EnemyCollisionHandler : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Runner"))
        {
            AudioManager.instance.PlayCollisionSound();

            Destroy(other.gameObject);
            Destroy(gameObject);

            PlayerManager playerManager = FindFirstObjectByType<PlayerManager>();
            if (playerManager != null)
            {
                playerManager.RemoveRunner(other.gameObject);
            }
        }
    }
}