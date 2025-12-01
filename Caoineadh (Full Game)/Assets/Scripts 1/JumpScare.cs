using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JumpScare : MonoBehaviour
{
    public Image jumpscare;
    public float scareTimer = 1.5f;
    public Vector3 startSize = new Vector3(0.1f, 0.1f, 0.1f);
    public Vector3 endSize = new Vector3(1f, 1f, 1f);
    public Vector3 startPos = new Vector3(0, 0, 0);
    public Vector3 endPos = new Vector3(0, 0, 0);
    public AudioSource scream;

    private void Start()
    {
        jumpscare.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(ShowJumpscare());
        }
    }

    private System.Collections.IEnumerator ShowJumpscare()
    {
        scream.Play();
        jumpscare.enabled = true;

        float timer = 0f;
        RectTransform rt = jumpscare.rectTransform;
        rt.localScale = startSize;
        rt.anchoredPosition = startPos;

        while (timer < scareTimer)
        {
            timer += Time.deltaTime;
            float t = timer / scareTimer;

            // Smooth step for nicer movement
            rt.localScale = Vector3.Lerp(startSize, endSize, Mathf.SmoothStep(0f, 1f, t));
            rt.anchoredPosition = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));

            yield return null;
        }

        yield return new WaitForSeconds(2);
        jumpscare.enabled = false;
    }
}
