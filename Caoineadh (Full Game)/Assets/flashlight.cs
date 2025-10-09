using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flashlight : MonoBehaviour
{
    public GameObject torch;
    public bool isOn;

    // Start is called before the first frame update
    void Start()
    {
        torch.SetActive(false);
        isOn = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Flashlight();
        }
    }

    public void Flashlight()
    {
        if (isOn == false)
        {
            torch.SetActive(true);
            isOn = true;
        }
        else if (isOn == true)
        {
            torch.SetActive(false);
            isOn = false;
        }
    }
}
