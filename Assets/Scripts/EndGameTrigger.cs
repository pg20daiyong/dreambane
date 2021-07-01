using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameTrigger : MonoBehaviour
{


    [SerializeField] Animator animator;

    [Header("Game Reset")]
    [SerializeField] float fadeSpeed;
    [SerializeField] float gameResetDelay;

    [Header("Character Movement")]
    [SerializeField] float movementTime;
    [SerializeField] float rotationSpeed;
    [SerializeField] float animationStopTime;

    [Header("Plant Scale")]
    [SerializeField] Transform plant;
    [SerializeField] Vector3 MaximumScale;
    [SerializeField] float growthTime;

    [Header("Light Intensity")]
    [SerializeField] Light pointLight;
    [SerializeField] float minimumBrightness, maximumBrightness;
    [SerializeField] float brightnessTime;

    float _waterPercent;

    bool _startedGrowth = false;

    Vector3 _minimumScale;
    Vector3 _finalSize;
    // Start is called before the first frame update
    void Awake()
    {
        _minimumScale = plant.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _waterPercent = (GameMgr.instance.leftBucket.waterRemaining + GameMgr.instance.rightBucket.waterRemaining) / 2;
        _finalSize = (_waterPercent / 100) * (MaximumScale - _minimumScale);
        _finalSize += _minimumScale;
        //move character to center of 
        GameMgr.instance.GameRunning = false;
        StartCoroutine(MovePlayerToCenter());
        StartCoroutine(RotatePlayerToPlant());
        //get water level and calculate growth percent

        ////print(_finalSize);




        //End game

        //Score??
    }
    IEnumerator MovePlayerToCenter()
    {
        float timeElapsed = 0;
        Transform startPos = GameMgr.instance.KitLocation;
        //Disable character movement
        while (timeElapsed <= movementTime)
        {
            timeElapsed += Time.deltaTime;
            GameMgr.instance.KitLocation.position = Vector3.Lerp(startPos.position, transform.position, timeElapsed / movementTime);
            if (timeElapsed >= animationStopTime && !_startedGrowth)
            {
                print("Animation Ended");
                GameMgr.instance.KitLocation.gameObject.GetComponent<CharacterAnimations>().Win();
                animator.SetBool("Growing", true);
                StartCoroutine(LightIncrease());
                StartCoroutine(PlantGrowth());
                _startedGrowth = true;
            }

            yield return null;
        }
    }

    IEnumerator RotatePlayerToPlant()
    {
        GameMgr.instance.KitLocation.gameObject.GetComponent<CharacterMovement3D>().DisableTurnControl();
        float timeElapsed = 0;
        //Disable character movement
        while (timeElapsed <= 10)
        {
            timeElapsed += Time.deltaTime;
            GameMgr.instance.KitLocation.rotation = Quaternion.RotateTowards(GameMgr.instance.KitLocation.rotation, Quaternion.Euler(0, 90, 0), timeElapsed * rotationSpeed);
            yield return null;
        }
    }

    IEnumerator PlantGrowth()
    {
        print("Plant Growth");
        float timeElapsed = 0;
        while (timeElapsed <= growthTime)
        {
            timeElapsed += Time.deltaTime;
            plant.localScale = Vector3.Lerp(_minimumScale, _finalSize, timeElapsed / growthTime);
            yield return null;
        }
        GameMgr.instance.gameOverBG.SetActive(true);
        GameMgr.instance.gameOverBG.GetComponent<ChangeTrasparency>().speed = fadeSpeed;
        StartCoroutine(GameReset());
    }


    IEnumerator LightIncrease()
    {
        float timeElapsed = 0;
        while (timeElapsed <= brightnessTime)
        {
            timeElapsed += Time.deltaTime;
            pointLight.intensity = Mathf.Lerp(minimumBrightness, maximumBrightness, timeElapsed / brightnessTime);
            yield return null;
        }
    }

    IEnumerator GameReset()
    {
        yield return new WaitForSeconds(gameResetDelay);
        GameMgr.instance.Reload();
    }
}
