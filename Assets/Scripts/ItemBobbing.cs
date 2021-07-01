using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBobbing : MonoBehaviour
{
    [Header("Item Bobbing")]
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _height = .5f;

    private Vector3 startPos;
    private Vector3 endPos;

    private float _heigtOffset;


    private void Start()
    {
        // stores the inital Y value in _heightOffset
        _heigtOffset = transform.position.y;
        startPos = transform.position;
        endPos = startPos + new Vector3(0, _height, 0);
    }
    void Update()
    {
        //stores the position in _position
        Vector3 _position = transform.position;
        // sets new position
        transform.position = new Vector3(_position.x, Mathf.PingPong(Time.time * _speed, _height) + _heigtOffset, _position.z);
    }

}
