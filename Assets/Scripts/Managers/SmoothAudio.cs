using UnityEngine;
using System.Collections;
public class SmoothAudio : MonoBehaviour
{
    public float delay = 1.5f;
    private void Awake()
    {
        G.smoothAudio = this;
        FadeIn(delay);
    }

    public IEnumerator ChangeVolume(float targetVolume, float duration)
    {
        float startVolume = AudioListener.volume;
        float time = 0f;

        targetVolume = Mathf.Clamp01(targetVolume);

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            AudioListener.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        AudioListener.volume = targetVolume;
    }

    public void FadeOut(float duration)
    {
        StartCoroutine(ChangeVolume(0f, duration));
    }

    public void FadeIn(float duration)
    {
        StartCoroutine(ChangeVolume(1f, duration));
    }
}
