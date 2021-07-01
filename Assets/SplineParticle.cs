using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(SplineWalker))]

public class SplineParticle : MonoBehaviour
{

    public static SplineParticle instance = null;

    public SplineWalker splineWalker { get; private set;}
    [SerializeField] float transitionTime = 1;
    public LerpItembobbing itemBobbing { get; private set; }

    private ParticleSystem[] particles;
    private AudioSource[] audioSources;


    public Vector3 rotationSpeed;
    private float convertedTime = 100;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        splineWalker = GetComponent<SplineWalker>();
        itemBobbing = GetComponent<LerpItembobbing>();
        particles = gameObject.GetComponentsInChildren<ParticleSystem>();
        audioSources = gameObject.GetComponentsInChildren<AudioSource>();
    }

    public void SetSpline(BezierSpline spline, float duration, SplineWalkerMode mode)
    {

        itemBobbing.StopBobbing();
        Destroy(splineWalker);
        splineWalker = gameObject.AddComponent<SplineWalker>();
        splineWalker.enabled = false;
        StartCoroutine(moveToNewSpline(spline, duration, mode));
    }


    public void Update()
    {
        if (!splineWalker.moving)
        {
            itemBobbing.StartBobbing();
        }
        //if (Input.GetKeyDown(KeyCode.F2)) StartCoroutine(Roation());
        //if (Input.GetKeyDown(KeyCode.F3)) StartEmmiting();

        float smooth = Time.deltaTime;
        transform.Rotate(rotationSpeed * smooth);
    }

    private IEnumerator moveToNewSpline(BezierSpline spline, float duration, SplineWalkerMode mode)
    {
        float timeElapsed = 0f;
        float SmoothTime = 1f;
        Vector3 startPos = transform.position;
        do
        {
            //transform size;
            transform.position = Vector3.Lerp(startPos, spline.GetPoint(0), timeElapsed / SmoothTime);
            timeElapsed += Time.deltaTime;
            yield return null;
        } while (timeElapsed <= SmoothTime);

        splineWalker.spline = spline;
        splineWalker.duration = duration;
        splineWalker.mode = mode;
        splineWalker.enabled = true;
    }

    public void StopEmmiting()
    {
        foreach(ParticleSystem particle in particles)
        {
            particle.enableEmission = false;
            gameObject.GetComponentInChildren<LightFlicker>().FadeOut();
            StartCoroutine(Fade(false));
        }
    }

    public void StartEmmiting()
    {
        foreach (ParticleSystem particle in particles)
        {
            particle.enableEmission = true;
            gameObject.GetComponentInChildren<LightFlicker>().FadeIn();
            StartCoroutine(Fade(true));
        }
    }

    private IEnumerator Fade(bool fadeIn)
    {

        float timeElapsed = 0;
        float totalTime = 5;
        while (timeElapsed <= totalTime)
        {
            timeElapsed += Time.deltaTime;
            if(fadeIn)
            {
                foreach (AudioSource audio in audioSources)
                {
                    audio.volume = Mathf.Lerp(0, 1, timeElapsed / totalTime);
                }
            }
            else
            {
                foreach (AudioSource audio in audioSources)
                {
                    audio.volume = Mathf.Lerp(1, 0, timeElapsed / totalTime);
                }
            }
            yield return null;
        }
    }

    private IEnumerator Roation()
    {
        float timeElapsed = 0;
        float totalTime = 10;
        while (true)
        {
            timeElapsed += Time.deltaTime;
            Quaternion roation = Quaternion.Euler(0, 0, 0);
            Quaternion fixedRotaion = Quaternion.Euler(360, 360, 360);
                    transform.rotation = Quaternion.Lerp(roation, fixedRotaion, timeElapsed / totalTime);
            yield return null;
        }
    }
}
