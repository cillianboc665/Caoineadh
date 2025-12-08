using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public class SFX : MonoBehaviour
{
    public AudioSource sfx;
    public Transform player;

    [TextArea] public string subtitleText;
    public bool showSubtitle = false;

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
            sfx.Play();

            if (showSubtitle && !string.IsNullOrEmpty(subtitleText))
            {
                Subtitles.Instance.ShowSubtitle(subtitleText, sfx.clip);
            }

            gameObject.SetActive(false);
        }
    }
}
