using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class textBox : MonoBehaviour
{
    public GameObject text;
    public string player = "Player";

    void Start()
    {
        if (text != null)
            text.SetActive(false);
    }

    private void Update()
    {
        if (GetComponent<BoxCollider>() != null && !(GetComponent<BoxCollider>()).enabled)
        {
            text.SetActive(false);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(player))
        {
            if (text != null)
                text.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(player))
        {
            if (text != null)
                text.SetActive(false);
        }
    }
}
