using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public Transform checkpointLocation;

    [Tooltip("Optional: If you want to disable multiple triggers at once")]
    public GameObject[] otherTriggersToDisable;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (checkpointLocation != null && CheckpointManager.Instance != null)
        {
            CheckpointManager.Instance.SetCheckpoint(checkpointLocation);

            // Disable this trigger
            gameObject.SetActive(false);

            // Optionally disable other triggers
            if (otherTriggersToDisable != null)
            {
                foreach (GameObject trigger in otherTriggersToDisable)
                {
                    if (trigger != null)
                        trigger.SetActive(false);
                }
            }

            Debug.Log($"Checkpoint '{checkpointLocation.name}' activated");
        }
        else
        {
            Debug.LogError("Checkpoint location or Manager is null!");
        }
    }
}