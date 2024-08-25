using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Headphonewarning : MonoBehaviour
{
    public Image headphonewarningImage;  // Reference to the Image component
    public float fadeDuration = 2f;      // Duration of the fade-out effect in seconds

    void Start()
    {
        // Ensure the image is fully visible at the start
        Color color = headphonewarningImage.color;
        color.a = 1f;
        headphonewarningImage.color = color;

        // Start the coroutine to fade out the image
        StartCoroutine(FadeOutCoroutine());
    }

    IEnumerator FadeOutCoroutine()
    {
        // Wait for 3 seconds before starting the fade
        yield return new WaitForSeconds(3);

        // Get the starting alpha value
        Color startColor = headphonewarningImage.color;
        float startAlpha = startColor.a;
        float elapsedTime = 0f;

        // Gradually fade out the image
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, 0f, elapsedTime / fadeDuration);
            Color newColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
            headphonewarningImage.color = newColor;
            yield return null;
        }

        // Ensure the alpha is exactly 0 after the fade is complete
        Color finalColor = headphonewarningImage.color;
        finalColor.a = 0f;
        headphonewarningImage.color = finalColor;

        // Optionally deactivate the GameObject after fading out
        headphonewarningImage.gameObject.SetActive(false);
    }
}
