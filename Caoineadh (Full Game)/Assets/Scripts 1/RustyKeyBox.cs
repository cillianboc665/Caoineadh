using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RustyKeyBox : MonoBehaviour
{
    public GameObject onLever1;
    public GameObject onLever2;
    public GameObject offLever1;

    public GameObject openLid;
    public GameObject closedLid;

    public GameObject key;

    public List<GameObject> leverScripts = new List<GameObject>();

    public AudioSource unlockSFX;

    // Start is called before the first frame update
    void Start()
    {
        openLid.SetActive(false);
        closedLid.SetActive(true);
        key.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (onLever1.activeInHierarchy && onLever2.activeInHierarchy && !offLever1.activeInHierarchy)
        {
            unlockSFX.Play();

            openLid.SetActive(true);
            closedLid.SetActive(false);
            key.SetActive(true);

            foreach (GameObject obj in leverScripts)
            {
                obj.GetComponent<BoxCollider>().enabled = false;
                obj.GetComponent<Lever>().enabled = false;
            }

            this.enabled = false;
        }
        else
        {
            openLid.SetActive(false);
            closedLid.SetActive(true);
            key.SetActive(false);
        }
    }
}
