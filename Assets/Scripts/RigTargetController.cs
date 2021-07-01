using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RigTargetController : MonoBehaviour
{

    [SerializeField] Rig twistRig;
    [SerializeField] Rig armsRig;

    // Update is called once per frame
    void Update()
    {
        if (!GameMgr.instance.GameRunning)
        {
            twistRig.weight = 0;
            armsRig.weight = 0;
        }
    }
}
