using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnlockDoor : MonoBehaviour
{
    private bool inRange = false;
    public GameObject doorToOpen;
    public TMP_Text doorText;
    public AudioSource sfx;
    public GameObject doorLock;

    // Start is called before the first frame update
    void Start()
    {
        doorText.text = "Requires Rusty Key";
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            sfx.Play();
            MonoBehaviour doorScript = doorToOpen.GetComponent<door>();
            doorScript.enabled = true;
            doorLock.SetActive(false);
            doorText.text = "'E' to interact";
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }
}
