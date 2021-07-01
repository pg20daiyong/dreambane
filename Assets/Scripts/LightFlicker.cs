using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class LightFlicker : MonoBehaviour
{

    [SerializeField] float minBrightness, maxBrightness;
    [SerializeField] float flickerRate = .1f;
    private Light _light;

    private float min, max;

    private void Awake()
    {
        _light = GetComponent<Light>();
        StartCoroutine(Flicker());
        min = minBrightness;
        max = maxBrightness;
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(false));
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(true));
    }

    private IEnumerator Flicker()
    {
        while(true)
        {
            _light.intensity = (Random.Range(min, max));
            yield return new WaitForSeconds(flickerRate);
        }
    }

    private IEnumerator Fade(bool fadeIn)
    {

        float timeElapsed = 0;
        float totalTime = 5;
        while (timeElapsed <= totalTime)
        {
            if(fadeIn)
            {
                timeElapsed += Time.deltaTime;
                max = Mathf.Lerp(0, maxBrightness, timeElapsed / totalTime);
                min = Mathf.Lerp(0, minBrightness, timeElapsed / totalTime);
            }
            else
            {
                timeElapsed += Time.deltaTime;
                max = Mathf.Lerp(maxBrightness, 0, timeElapsed / totalTime);
                min = Mathf.Lerp(min, 0, timeElapsed / totalTime);
            }
            yield return null;
        }
    }
}
