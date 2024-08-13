using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public UnityEvent onFadeComplete;

    public bool isFading = false;

    public bool fadeInStart = true;
    public float fadeInStartDuration = 1.5f;

    private void Start()
    {
        // Ensure the fade image is fully transparent at start
        SetAlpha(0);

        if(fadeInStart)
        {
            FadeIn(fadeInStartDuration);
        }
    }

    public void FadeToBlack(float duration)
    {
        StartCoroutine(Fade(1, duration));
    }

    public void FadeIn(float duration)
    {
        StartCoroutine(Fade(0, duration));
    }

    private IEnumerator Fade(float targetAlpha, float duration)
    {
        isFading = true;
        float startAlpha = fadeImage.color.a;
        float time = 0;

        while (time < duration)
        {
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / duration);
            SetAlpha(alpha);
            time += Time.deltaTime;
            yield return null;
        }

        SetAlpha(targetAlpha);
        isFading = false;
        onFadeComplete.Invoke();
    }

    private void SetAlpha(float alpha)
    {
        Color color = fadeImage.color;
        color.a = alpha;
        fadeImage.color = color;
    }
}
