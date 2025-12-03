using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WallText : MonoBehaviour
{
    public GameObject nightVision;
    public TextMeshPro wallText;

    // Start is called before the first frame update
    void Start()
    {
        wallText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (nightVision.activeInHierarchy)
        {
            wallText.enabled = true;
        }
        else
        {
            wallText.enabled = false;
        }
    }
}
