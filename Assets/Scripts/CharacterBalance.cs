using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterBalance : MonoBehaviour
{

    [SerializeField] private float balancingPower = 5;
    [SerializeField] private float movementForce = 1;
    [SerializeField] private float deathForce = 50f;
    private Vector2 _moveInput, _balancingInput;
    private float xForce, zForce;

    private float xMovement, zMovement;


    public void OnMove(InputValue value)
    {
        if (!GameMgr.instance.GameRunning) return;
        _moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        if (!GameMgr.instance.GameRunning) return;
        ApplyRotation(value);
    }

    public void AddFallingForce(float force)
    {
        if (force == 0)
            return;
        if(force >= deathForce)
        {
            GameMgr.instance.GameOver();
        }
        Yoke.instance.Rotate(force, force);
    }

    private void ApplyRotation(InputValue value)
    {
        if (!GameMgr.instance.GameRunning) return;
        _balancingInput = value.Get<Vector2>();

        xMovement = (_balancingInput.y * gameObject.transform.forward.x) + (-_balancingInput.x * gameObject.transform.forward.z) * balancingPower;
        zMovement = (_balancingInput.x * gameObject.transform.forward.x) + (_balancingInput.y * gameObject.transform.forward.z) * balancingPower;

        //y is forward and back/ x is left and right
        Yoke.instance.Rotate(xMovement, zMovement);
    }

    private void Update()
    {
        if (!GameMgr.instance.GameRunning) return;

        Yoke.instance.yRotation = transform.rotation.eulerAngles.y;

        xForce = (-_moveInput.x * gameObject.transform.forward.x) + (-_balancingInput.x * gameObject.transform.forward.z) * movementForce;
        zForce = (-_moveInput.x * gameObject.transform.forward.x) * movementForce;

        Yoke.instance.Rotate(zForce, xForce);

    }


}
