using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class flashlight : MonoBehaviour
{
    public GameObject torch;
    public bool isOn;

    public float useTime = 10f;
    public float chargeTime = 10f;
    public float charge = 100f;

    public TextMeshProUGUI lightText;
    public TextMeshProUGUI useableText;

    public float toggleCooldown = 0.3f;
    private float lastToggleTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        torch.SetActive(false);
        isOn = false;
        UI();
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F) && Time.time - lastToggleTime >= toggleCooldown)
        {
            lastToggleTime = Time.time;

            if (isOn == true)
            {
                torch.SetActive(false);
                isOn = false;
            }
            else if (charge >= 0.05f)
            {
                torch.SetActive(true);
                isOn = true;
            }
        }

        if (isOn)
        {
            charge -= useTime * Time.deltaTime;
            if (charge <= 0f)
            {
                charge = 0f;
                torch.SetActive(false);
                isOn = false;
            }
        }
        else
        {
            if (charge < 100f)
            {
                charge += chargeTime * Time.deltaTime;
                if (charge > 100f)
                {
                    charge = 100f;
                }
            }
        }

        UI();
    }

    public void UI()
    {
        lightText.text = $"Charge {charge:F0}%";

        if (isOn)
        {
            useableText.color = new Color(1f, 0f, 0f, useableText.color.a);
        }
        else
        {
            useableText.color = new Color(0f, 1f, 0f, useableText.color.a);

        }
    }
}
