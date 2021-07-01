using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeTrasparency : MonoBehaviour
{
    GameMgr gameMgr;
    public GameObject bg;

    private float alpha = 0.01f;
    public float speed = 5.0f;
    public  Image currentImg;

    void Start()
    {
        bg = gameObject;
        
        float alpha = currentImg.color.a;
    }

    void Update()
    {
       
        
        if (!GameMgr.instance.GameRunning && alpha < 1)
        {
            alpha += Time.deltaTime* speed;
            Debug.Log(alpha);
            Color newColor = new Color(0, 0, 0, alpha);
            currentImg.color = newColor;

        }
        
  

        
    }



    //void ChangeAlpha(Material mat, float alphaVal)
    //{
    //    Color oldcolor = mat.color;
    //    Color newColor = new Color(oldcolor.r, oldcolor.g, oldcolor.b, alphaVal);
    //    mat.SetColor("_Color", newColor);
    //}
}
