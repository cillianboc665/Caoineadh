using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using static Unity.VisualScripting.Metadata;
using static UnityEngine.Rendering.DebugUI.Table;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;
using UnityEngine.XR;
public class EyeBlob : MonoBehaviour
{
    [Header("References")]
    public Transform blob;      // The BlobMeshObject (deforming mesh)
    public Transform player;    // Optional: the player to look at

    [Header("Settings")]
    public bool lookAtPlayer = true;  // Should eyes rotate toward player?

    private Vector3 offset;      // Initial offset from blob center

    void Start()
    {
        if (blob == null)
        {
            Debug.LogError($"{name}: Blob reference not assigned!");
            return;
        }

        // Calculate initial offset from the blob's center
        offset = transform.position - blob.position;

        // If player not assigned, try to find it by tag
        if (lookAtPlayer && player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    }

    void LateUpdate()
    {
        if (blob == null)
            return;

        // Move eye to maintain offset from blob center
        transform.position = blob.position + offset;

        // Optionally rotate eye toward player
        if (lookAtPlayer && player != null)
        {
            transform.LookAt(player.position);
        }
    }
}
