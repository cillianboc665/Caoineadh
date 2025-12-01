using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public GameObject Door;
    public float openAngle = 90f;
    public float closedAngle = 0f;
    public float speed = 2f;
    public bool open = false;
    public Transform player;
    public float activeDist = 3f;
    public AudioSource opening;
    public AudioSource closing;

    public Transform enemy;
    private bool openedByEnemy = false;

    float NormalizeAngle(float angle)
    {
        angle = angle % 360;
        if (angle > 180) angle -= 360;
        return angle;
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector3.Distance(player.position, transform.position);
        float distToEnemy = Vector3.Distance(enemy.position, transform.position);


        Vector3 currentRot = Door.transform.localEulerAngles;
        float currentY = NormalizeAngle(currentRot.y);

        bool wasOpen = open;

        if (dist <= activeDist && Input.GetKeyDown(KeyCode.E))
        {
            open = !open;
            openedByEnemy = false;
        }

        if (distToEnemy <= activeDist)
        {
            open = true;
            openedByEnemy = true;
        }

        if (openedByEnemy && distToEnemy > activeDist)
        {
            open = false;
            openedByEnemy = false;
        }

        if (open && !wasOpen)
            opening.Play();
        else if (!open && wasOpen)
            closing.Play();

        if (open)
        {
            float newY = Mathf.LerpAngle(currentY, openAngle, speed * Time.deltaTime);
            Door.transform.localEulerAngles = new Vector3(currentRot.x, newY, currentRot.z);
        }
        else
        {
            float newY = Mathf.LerpAngle(currentY, closedAngle, speed * Time.deltaTime);
            Door.transform.localEulerAngles = new Vector3(currentRot.x, newY, currentRot.z);
        }
    }

    public void ToggleDoor()
    {
        open = !open;
    }
}
