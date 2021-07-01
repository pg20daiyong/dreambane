using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DebugController : MonoBehaviour
{

    [SerializeField] private GameObject DebugMenu;

    [SerializeField] private UIToggle Toggle1, Toggle2;

    [SerializeField] private Transform KitLocation;

    [SerializeField] private GameObject[] Checkpoints;
    // Start is called before the first frame update
    // Update is called once per frame

    private bool Active = false;
    private LinkedList<GameObject> checkPoints;

    private int currentPoint = 0;
    private void Start()
    {
        checkPoints = new LinkedList<GameObject>(Checkpoints);
    }


    private void Update()
    {

        Debug.Log(GameMgr.instance.DebugMode);
        if (!GameMgr.instance.DebugMode)
            return;

        if(Input.GetKeyDown(KeyCode.N))
        {
            if (Toggle1.Toggle())
            {
                GameMgr.instance.leftBucket.DebugEnabled = true;
                GameMgr.instance.rightBucket.DebugEnabled = true;
                return;
            }
            GameMgr.instance.leftBucket.DebugEnabled = false;
            GameMgr.instance.rightBucket.DebugEnabled = false;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            //TODO make this so it doesn't restart when moving through checkpoint
            if (Toggle2.Toggle())
            {
                GameMgr.instance.leftFeeze.TimerGoing = false;
                return;
            }
            GameMgr.instance.leftFeeze.TimerGoing = true;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
           GameMgr.instance.AddWaterToBucket(100);
        }
        if(Input.GetKeyDown(KeyCode.P))
        {
            LinkedListNode<GameObject> node = checkPoints.First;
            currentPoint++;
            currentPoint = Mathf.Clamp(currentPoint, 0, checkPoints.Count - 1);
            for (int i = 0; i < currentPoint; i++)
            {
                node = node.Next;
            }
            if(KitLocation == null)
            {
                Debug.Log("KIT PREFAB NOT SET IN DEBUG CONTROLLER");
                return;
            }
            KitLocation.position = node.Value.transform.position;

        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            LinkedListNode<GameObject> node = checkPoints.First;
            currentPoint--;
            currentPoint = Mathf.Clamp(currentPoint, 0, checkPoints.Count - 1);

            for (int i = 0; i < currentPoint; i++)
            {
                node = node.Next;
            }
            if (KitLocation == null)
            {
                Debug.Log("KIT PREFAB NOT SET IN DEBUG CONTROLLER");
                return;
            }
            KitLocation.position = node.Value.transform.position;

        }
        //TODO Add teleport, check in with Yurii for what he wants first.
    }
}
