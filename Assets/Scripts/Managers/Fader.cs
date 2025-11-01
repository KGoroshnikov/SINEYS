using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fader : MonoBehaviour
{
    private Image fadeImage;

    private void Awake()
    {
        G.fader = this;
    }

    private IEnumerator Start()
    {
        fadeImage = GetComponent<Image>();
        fadeImage.color = Color.black;
        yield return new WaitForSeconds(0.25f);
        FadeOut(1f);
    }

    public void FadeIn(float delay = 1f, float targetAlpha = 1f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeTo(targetAlpha, delay));
    }

    public void FadeOut(float delay = 1f)
    {
        StopAllCoroutines();
        StartCoroutine(FadeTo(0f, delay));
    }

    private IEnumerator FadeTo(float targetAlpha, float delay)
    {
        if (fadeImage == null)
            yield break;

        Color color = fadeImage.color;
        float startAlpha = color.a;
        float timeElapsed = 0f;

        while (timeElapsed < delay)
        {
            timeElapsed += Time.deltaTime;
            float t = Mathf.Clamp01(timeElapsed / delay);
            color.a = Mathf.Lerp(startAlpha, targetAlpha, t);
            fadeImage.color = color;
            yield return null;
        }
        color.a = targetAlpha;
        fadeImage.color = color;
    }
}
