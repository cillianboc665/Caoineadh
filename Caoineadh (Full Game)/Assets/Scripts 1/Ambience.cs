using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambience : MonoBehaviour
{
    public AudioSource outside;
    public AudioSource inside;
    public Transform player;
    public bool outPlaying;

    // Start is called before the first frame update
    void Start()
    {
        outside.Play();
        outPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            if (outPlaying == true)
            {
                outside.Stop();
                inside.Play();
                outPlaying = false;
            }

            else if (outPlaying == false)
            {
                inside.Stop();
                outside.Play();
                outPlaying = true;
            }
        }
    }
}
