using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
using UnityEngine.Rendering;
using UnityEngine.Animations.Rigging;
using UnityEngine.Rendering.Universal;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameMgr : Singleton<GameMgr>
{

    public static GameMgr instance = null;

    public Bucket leftBucket, rightBucket;
    public WaterFreezeTimer leftFeeze, rightFreeze;

    [Header("Game Start")]
    [SerializeField] CinemachineVirtualCamera gameCam, menuCam;
    [SerializeField] private VolumeProfile _profile;
    [SerializeField] private float _delay = 1.5f;

    [Header("Left HUD")]
    [SerializeField] Image leftWaterBar;
    [SerializeField] Image leftIceBar;

    [Header("Right HUD")]
    [SerializeField] Image rightWaterBar;
    [SerializeField] Image rightIceBar;

    [SerializeField] private GameObject headTarget;
    [SerializeField] private CharacterAnimations kitAnims;
    private float leftCurrentWater, rightCurrentWater;
    private float leftFrozenPercent, rightFrozenPercent;

    [Header("Camera Transition")]
    [SerializeField] float menuFocusDistance;
    [SerializeField] float gameFocusDistance;
    [SerializeField] float transitionTime;


    public bool GameRunning { get;  set; } = false;
    public bool GamePaused { get; private set; } = false;
    public bool DebugMode { get; set; } = false;

    public Transform KitLocation {get; set; }

    [Header("UI HUD")]
    [SerializeField] private GameObject gameHUD;
    [SerializeField] private float _initialDelay;
    [SerializeField] public GameObject gameOverBG;
    [SerializeField] private Animator deathAnimator;
    [SerializeField] private string animationName;
    [SerializeField] private string _sceneToLoad;
    [SerializeField] private float _deathDelay = 2.167f;
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject pauseAction;


    private bool zoomedIn = true;
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        if (_profile.TryGet(out DepthOfField depth))
        {
            depth.focusDistance.value = 6;
        }
        KitLocation = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        Cursor.lockState = CursorLockMode.None;
    }

    private void FixedUpdate()
    {
        UpdateWaterRemaining();
        HandleHealthBar();
    }

    //updates current water for each bucket with the data from each bucket. 
    public void UpdateWaterRemaining()
    {
        leftCurrentWater = (leftBucket.waterRemaining)/100;
        leftFrozenPercent = Mathf.Clamp(leftFeeze.FrozenPercent, 0f, leftCurrentWater);

        rightCurrentWater = (rightBucket.waterRemaining )/ 100;
        rightFrozenPercent = Mathf.Clamp(rightFreeze.FrozenPercent, 0f, rightCurrentWater);


        float leftUnfrozenWater = leftCurrentWater - leftFrozenPercent;
        float rightUnfrozenWater = rightCurrentWater - rightFrozenPercent;
        if (leftUnfrozenWater == 0 && rightUnfrozenWater == 0)
        {
            GameOver();
        }
    }

    //adds water directly to bucket objects
    public void AddWaterToBucket(float amountToFill)
    {
        leftBucket.FillWater(amountToFill);
        rightBucket.FillWater(amountToFill);
    }

    //Updates hud elements for buckets
    private void HandleHealthBar()
    {
        leftWaterBar.fillAmount = leftCurrentWater;
        leftIceBar.fillAmount = leftFrozenPercent;

        rightWaterBar.fillAmount = rightCurrentWater;
        rightIceBar.fillAmount = rightFrozenPercent;
    }

    public void GameOver()
    {
        if (GameRunning)
        {
            kitAnims.Die();
            GameRunning = false;
            StartCoroutine(DeathDelay());
            pauseAction.SetActive(false);
            Cursor.lockState = CursorLockMode.None;
            StartCoroutine(ResetDelay());
        }
    }    

    public void StartGame()
    {

        //set new camera
        gameCam.Priority = 20;
        menuCam.Priority = 0;
        //set new depth of field

        if (_profile.TryGet(out DepthOfField depth))
        {
            depth.focusDistance.value = 10;
            Debug.Log("Focal Length: " + depth.focusDistance);
        }
        StartCoroutine(GameStartDelay());        
    }

    public void Reload()
    {
        SceneManager.LoadScene(_sceneToLoad);
    }

    private IEnumerator DeathDelay()
    {
        yield return new WaitForSeconds(_initialDelay);
        gameOverBG.SetActive(true);
        gameOverScreen.SetActive(true);
        deathAnimator.Play(animationName);
    }

    private IEnumerator ResetDelay()
    {
        yield return new WaitForSeconds(_deathDelay);
        SceneManager.LoadScene(_sceneToLoad);
    }
    private IEnumerator GameStartDelay()
    {
        yield return new WaitForSeconds(_delay);
        float timeElapsed = 0f;
        float strength = 0;
        StartCoroutine(Fade());
        //stand up && enable controls
        GameRunning = true;
        gameHUD.SetActive(true);
        leftFeeze.BeginTimer();
        do
        {
            //transform size;
            strength = Mathf.Lerp(0, 1, timeElapsed / _delay);
            timeElapsed += Time.deltaTime;
            headTarget.GetComponent<Rig>().weight = strength;
            timeElapsed += Time.deltaTime;
            yield return null;
        } while (timeElapsed <= _delay);
    }

    IEnumerator Fade()
    {
        if (_profile.TryGet(out DepthOfField depth))
        {
            float currentFocus;
            float currentTime = 0f;
            if (zoomedIn)
            {
                do
                {
                    currentFocus = Mathf.Lerp(menuFocusDistance, gameFocusDistance, currentTime / transitionTime);
                    currentTime += Time.deltaTime;
                    depth.focusDistance.value = currentFocus;
                    yield return null;
                } while (currentTime <= transitionTime);
                zoomedIn = false;
            }
            else
            {
                do
                {
                    currentFocus = Mathf.Lerp(gameFocusDistance, menuFocusDistance, currentTime / transitionTime);
                    currentTime += Time.deltaTime;
                    depth.focusDistance.value = currentFocus;
                    yield return null;
                } while (currentTime <= transitionTime);
                zoomedIn = true;
            }
        }
        yield return null;
    }

    public void PauseGame()
    {
        // transition to menuCam
        gameCam.Priority = 0;
        menuCam.Priority = 20;
        GamePaused = true;
    }

    public void ResumeGame()
    {
        // transition to gameCam
        gameCam.Priority = 20;
        menuCam.Priority = 0;
        GamePaused = false;
    }
}
