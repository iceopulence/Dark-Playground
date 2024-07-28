using UnityEngine;
using System.Collections;

public class LightFlicker : MonoBehaviour
{
    public Light flickerLight;

    public float minIntensity = 0.5f;
    public float maxIntensity = 2.0f;
    public float minFlickerDuration = 0.1f;
    public float maxFlickerDuration = 0.5f;
    public float minIntervalDuration = 0.1f;
    public float maxIntervalDuration = 0.5f;
    public float lerpSpeed = 1.0f;  // Adjust this to control the speed of lerping

    private float nextFlickerTime = 0f;

    void Update()
    {
        if (Time.time >= nextFlickerTime)
        {
            StartCoroutine(Flicker());
            SetNextFlickerTime();
        }
    }

    private void SetNextFlickerTime()
    {
        nextFlickerTime = Time.time + Random.Range(minIntervalDuration, maxIntervalDuration);
    }

    private IEnumerator Flicker()
    {
        float flickerDuration = Random.Range(minFlickerDuration, maxFlickerDuration);
        float endTime = Time.time + flickerDuration;
        float startIntensity = flickerLight.intensity;
        float targetIntensity = Random.Range(minIntensity, maxIntensity);

        float progress = 0f; // Progress of the lerp

        while (progress < 1f)
        {
            progress += Time.deltaTime * lerpSpeed / flickerDuration;
            flickerLight.intensity = Mathf.Lerp(startIntensity, targetIntensity, progress);
            yield return null;
        }

        flickerLight.intensity = minIntensity;  // reset to minimum intensity after flickering
    }
}
