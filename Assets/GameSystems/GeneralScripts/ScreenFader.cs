using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour
{
    public Image fadeImage;
    public UnityEvent onFadeComplete;

    public bool isFading = false;

    public bool fadeInStart = true;
    public float fadeInStartDuration = 1.5f;

    private void Awake()
    {
        // Ensure the fade image is fully transparent at start
        SetAlpha(0);

        if (fadeInStart)
        {
            FadeIn(fadeInStartDuration);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FadeIn(fadeInStartDuration);
    }

    public void FadeToBlack(float duration)
    {
        SetAlpha(0);
        StartCoroutine(Fade(1, duration));
    }

    public void FadeIn(float duration)
    {
        SetAlpha(1);
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
