using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Transform lever;
    public bool leverOn;
    private bool inRange = false;

    public GameObject leverUp;
    public GameObject leverDown;

    // Start is called before the first frame update
    void Start()
    {
        leverOn = false;
        leverUp.SetActive(true);
        leverDown.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (inRange && Input.GetKeyDown(KeyCode.E))
        {
            if (leverOn)
            {
                leverOn = false;
                leverUp.SetActive(true);
                leverDown.SetActive(false);
            }
            else if (!leverOn)
            {
                leverOn = true;
                leverUp.SetActive(false);
                leverDown.SetActive(true);
            }
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
