using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudController : MonoBehaviour
{
    public static HudController instance = null;

    private bool mouseLock = false;
    public enum HudStates
    {
        Menu,
        Paused,
        Running,
        GameOver
    }

    [SerializeField] GameObject GameScreen;
    [SerializeField] GameObject DebugScreen;
    [SerializeField] GameObject GameOverScreen;

    public HudStates _currentState { get; set; } = HudStates.Menu;
    //Create singlton object


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);
    }

    private void Update()
    {

        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameMgr.instance.DebugMode)
            {
                GameMgr.instance.DebugMode = false;
                DebugScreen.SetActive(false);
                return;
            }
            GameMgr.instance.DebugMode = true;
            DebugScreen.SetActive(true);
        }

        if (!GameMgr.instance.GameRunning)
        {
            GameScreen.SetActive(false);
        }
    }
}
