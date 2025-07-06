using PathCreation;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    public PathCreator pathCreator;
    public EndOfPathInstruction endOfPathInstruction;
    public float speed = 5;
    public float baseOffset = -1f;
    private float inputOffset = 0f;
    private float distanceTravelled;

    private PlayerManager playerManager;
    private bool hasReachedEnd = false;

    void Start()
    {
        playerManager = GetComponent<PlayerManager>();

        if (pathCreator != null)
        {
            pathCreator.pathUpdated += OnPathChanged;
        }
    }

    void Update()
    {
        if (pathCreator != null && !hasReachedEnd && GameManager.instance != null && GameManager.instance.IsGameStarted)
        {
            distanceTravelled += speed * Time.deltaTime;
            Vector3 pathPosition = pathCreator.path.GetPointAtDistance(distanceTravelled, endOfPathInstruction);
            Quaternion pathRotation = pathCreator.path.GetRotationAtDistance(distanceTravelled, endOfPathInstruction);
            float totalOffset = baseOffset + inputOffset;
            transform.position = pathPosition + (pathRotation * Vector3.down) * totalOffset;
            transform.rotation = pathRotation * Quaternion.Euler(5, 0, 90);
        }

        if (!hasReachedEnd && playerManager != null && pathCreator != null)
        {
            float frontRunnerDistance = playerManager.GetFrontRunnerDistance(pathCreator);

            if (frontRunnerDistance >= pathCreator.path.length)
            {
                hasReachedEnd = true;

                playerManager.FreezeAllRunners();

                this.speed = 0;

                Debug.Log("Front-most runner reached the end! Freezing all.");
            }
        }
    }

    void OnPathChanged()
    {
        distanceTravelled = pathCreator.path.GetClosestDistanceAlongPath(transform.position);
    }

    public void SetInputOffset(float newOffset)
    {
        inputOffset = newOffset;
    }
}