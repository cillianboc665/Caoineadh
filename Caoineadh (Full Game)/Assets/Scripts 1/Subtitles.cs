using System.Collections;
using TMPro;
using UnityEngine;

public class Subtitles : MonoBehaviour
{
    public static Subtitles Instance;

    public TextMeshProUGUI subtitleText;

    private Coroutine currentCoroutine;

    private void Awake()
    {
        Instance = this;
        subtitleText.text = "";
    }

    public void ShowSubtitle(string text, AudioClip clip)
    {
        if (currentCoroutine != null)
            StopCoroutine(currentCoroutine);

        currentCoroutine = StartCoroutine(SubtitleRoutine(text, clip));
    }

    private IEnumerator SubtitleRoutine(string text, AudioClip clip)
    {
        float duration = clip.length;

        yield return StartCoroutine(TypeLine(text, duration));

        // Clear after typing ends (optional: add a delay)
        subtitleText.text = "";
        currentCoroutine = null;
    }

    private IEnumerator TypeLine(string line, float duration)
    {
        subtitleText.text = "";

        int totalChars = line.Length;

        if (totalChars == 0)
            yield break;

        float timePerChar = duration / totalChars;

        foreach (char c in line)
        {
            subtitleText.text += c;

            if (PauseMenu.isPaused)
            {
                while (PauseMenu.isPaused)
                    yield return null;
            }

            yield return new WaitForSeconds(timePerChar);
        }
    }
}