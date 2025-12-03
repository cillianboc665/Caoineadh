using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextColour : MonoBehaviour
{
    public GameObject nightVision;
    public TextMeshPro textBox;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (nightVision.activeInHierarchy)
        {
            textBox.color = Color.black;
        }
        else
        {
            textBox.color = Color.white;
        }
    }
}
