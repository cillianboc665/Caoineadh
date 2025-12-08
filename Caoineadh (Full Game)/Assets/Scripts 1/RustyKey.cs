using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RustyKey : MonoBehaviour
{
    private bool inRange = false;
    public GameObject doorLock;
    public GameObject key;
    public AudioSource sfx;

    // Start is called before the first frame update
    void Start()
    {
        doorLock.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            sfx.Play();
            doorLock.SetActive(true);
            key.SetActive(false);
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
