using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambience : MonoBehaviour
{
    public AudioSource outside;
    public AudioSource inside;
    public Transform player;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            outside.Stop();
            inside.Play();

        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.transform == player)
        {
            outside.Play();
            inside.Stop();
        }
    }
}
