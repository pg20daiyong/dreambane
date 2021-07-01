using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenFreeze : MonoBehaviour
{
    [SerializeField] private float minAlphaCutOff, maxAlphaCutOff;

    [Range(1,2)]
    [SerializeField] private float Offset = 1.25f;
    private Renderer iceMat;
    private void Start()
    {
        iceMat = GetComponent<Renderer>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        float totalFrozen = 1- (GameMgr.instance.leftFeeze.FrozenPercent + GameMgr.instance.rightFreeze.FrozenPercent) / 2;
        totalFrozen = Mathf.Clamp(totalFrozen, minAlphaCutOff, maxAlphaCutOff);
        iceMat.material.SetFloat("AlphaClipping", totalFrozen);
    }
}
