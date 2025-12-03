using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    public Transform currentCheckpoint;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        if (checkpoint != null)
        {
            currentCheckpoint = checkpoint;
            Debug.Log("Checkpoint set to: " + checkpoint.name);
        }
    }

    public void TeleportPlayer(Transform player)
    {
        if (player == null)
        {
            Debug.LogError("Player transform is null!");
            return;
        }

        if (currentCheckpoint == null)
        {
            Debug.LogError("No checkpoint set! Looking for any active checkpoint...");

            // Try to find any checkpoint in the scene
            GameObject[] allCheckpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
            if (allCheckpoints.Length > 0)
            {
                currentCheckpoint = allCheckpoints[0].transform;
                Debug.Log("Found fallback checkpoint: " + currentCheckpoint.name);
            }
            else
            {
                Debug.LogError("No checkpoints found in scene!");
                return;
            }
        }

        Debug.Log($"Teleporting player to: {currentCheckpoint.name} at {currentCheckpoint.position}");

        // Method 1: Simple teleport (works for most cases)
        player.position = currentCheckpoint.position;

        // Method 2: Alternative with Rigidbody
        /*
        Rigidbody rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            // Store velocity state
            bool wasKinematic = rb.isKinematic;
            Vector3 oldVelocity = rb.velocity;
            Vector3 oldAngularVelocity = rb.angularVelocity;
            
            // Teleport
            rb.position = currentCheckpoint.position;
            
            // Reset velocity to prevent physics issues
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            
            // Restore kinematic state
            rb.isKinematic = wasKinematic;
        }
        else
        {
            player.position = currentCheckpoint.position;
        }
        */

        // Optional: Reset player rotation
        // player.rotation = currentCheckpoint.rotation;

        Debug.Log("Teleport complete!");
    }
}