using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class JumpScare : MonoBehaviour
{
    [Header("Jumpscare Settings")]
    public Image jumpscare;
    public GameObject panel;
    public float scareTimer = 1.5f;
    public Vector3 startSize = new Vector3(0.1f, 0.1f, 0.1f);
    public Vector3 endSize = new Vector3(1f, 1f, 1f);
    public Vector3 startPos = new Vector3(0, 0, 0);
    public Vector3 endPos = new Vector3(0, 0, 0);
    public AudioSource scream;

    [Header("Player Components")]
    public PlayerMovement playerMovement;
    public CameraShake cameraShake;
    public Transform player;
    public Rigidbody playerRigidbody;

    [Header("Environment")]
    public EnemyAI enemyAI;
    public AudioSource indoorAmbience;
    public AudioSource outdoorAmbience;
    public Canvas ui;

    private bool isScaring = false;

    private void Start()
    {
        jumpscare.enabled = false;
        panel.SetActive(false);

        // Auto-find references if not set
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
                playerRigidbody = playerObj.GetComponent<Rigidbody>();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (isScaring) return;

        isScaring = true;
        StartCoroutine(ShowJumpscare());
    }

    private IEnumerator ShowJumpscare()
    {
        // Disable player control and other systems
        if (playerMovement != null) playerMovement.enabled = false;
        if (cameraShake != null) cameraShake.enabled = false;
        if (enemyAI != null) enemyAI.enabled = false;
        if (indoorAmbience != null) indoorAmbience.enabled = false;
        if (outdoorAmbience != null) outdoorAmbience.enabled = false;
        if (ui != null) ui.enabled = false;

        // If player has rigidbody, temporarily disable physics
        if (playerRigidbody != null)
        {
            playerRigidbody.velocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
        }

        // Play jumpscare
        if (scream != null) scream.Play();
        panel.SetActive(true);
        jumpscare.enabled = true;

        // Animate jumpscare
        float timer = 0f;
        RectTransform rt = jumpscare.rectTransform;
        rt.localScale = startSize;
        rt.anchoredPosition = startPos;

        while (timer < scareTimer)
        {
            timer += Time.deltaTime;
            float t = timer / scareTimer;
            rt.localScale = Vector3.Lerp(startSize, endSize, Mathf.SmoothStep(0f, 1f, t));
            rt.anchoredPosition = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }

        yield return new WaitForSeconds(2);

        // TELEPORT PLAYER
        TeleportPlayerToCheckpoint();

        // Small delay to ensure teleport completes
        yield return new WaitForSeconds(0.1f);

        // Re-enable everything
        if (playerMovement != null) playerMovement.enabled = true;
        if (cameraShake != null) cameraShake.enabled = true;
        if (enemyAI != null) enemyAI.enabled = true;
        if (indoorAmbience != null) indoorAmbience.enabled = true;
        if (outdoorAmbience != null) outdoorAmbience.enabled = true;
        if (ui != null) ui.enabled = true;

        jumpscare.enabled = false;
        panel.SetActive(false);
        isScaring = false;
    }

    private void TeleportPlayerToCheckpoint()
    {
        if (player == null)
        {
            Debug.LogError("Player transform is null!");
            return;
        }

        if (CheckpointManager.Instance == null)
        {
            Debug.LogError("CheckpointManager instance is null!");
            return;
        }

        // Simple direct teleport - most reliable
        Transform checkpoint = GetActiveCheckpoint();
        if (checkpoint != null)
        {
            Debug.Log($"Teleporting player to checkpoint: {checkpoint.name}");
            player.position = checkpoint.position;

            // If you have character controller
            CharacterController charController = player.GetComponent<CharacterController>();
            if (charController != null)
            {
                charController.enabled = false;
                player.position = checkpoint.position;
                charController.enabled = true;
            }
        }
        else
        {
            Debug.LogError("No checkpoint found!");
        }
    }

    private Transform GetActiveCheckpoint()
    {
        // First try the manager
        if (CheckpointManager.Instance.currentCheckpoint != null)
            return CheckpointManager.Instance.currentCheckpoint;

        // Fallback: Find any checkpoint in scene
        GameObject[] checkpoints = GameObject.FindGameObjectsWithTag("Checkpoint");
        if (checkpoints.Length > 0)
            return checkpoints[0].transform;

        return null;
    }
}