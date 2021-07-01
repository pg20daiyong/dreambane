using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Animations.Rigging;
#pragma warning disable 649

// sends input from PlayerInput to attached CharacterMovement components
public class PlayerController : MonoBehaviour
{
    // initial cursor state
    [SerializeField] private CursorLockMode _cursorMode = CursorLockMode.None;
    // make character look in Camera direction instead of MoveDirection
    [SerializeField] private bool _lookInCameraDirection;

    // Manage health, score and etc.
    [SerializeField] private BasicAudioPlayer basicAudioPlayer;
    [SerializeField] private AudioClip jump, land, yorkScreech;
    [SerializeField] private float cooldown = 0.5f;
    [SerializeField] private GameObject handsRig;
    [SerializeField] private LayerMask grabLayer;

    [SerializeField] private float jumpDelay;

    private CharacterMovement _characterMovement;
    private CharacterAnimations _characterAnimations;
    private JumpAudio _jumpAudio;
    private GameObject currentDrag;
    private Vector2 _moveInput;
    private bool _grabbing = false;
    private bool _hasLanded = true;
    private Vector2 _prevMoveInput;

    private float Timer = 0.0f;

    private void Awake()
    {
        _characterMovement = GetComponent<CharacterMovement>();
        _characterAnimations = GetComponent<CharacterAnimations>();
        _jumpAudio = GetComponent<JumpAudio>();
    }

    private void OnGrab(InputValue value)
    {
        if (!GameMgr.instance.GameRunning) return;
        RaycastHit hit;
        if(Physics.Raycast(transform.position + new Vector3(0,0.5f), gameObject.transform.forward, out hit, 0.4f, grabLayer) && !_grabbing)
        {
            handsRig.GetComponent<Rig>().weight = 1;
            _grabbing = true;
            Yoke.instance.isGrabbing = true;
            currentDrag = hit.collider.gameObject;
            currentDrag.AddComponent<FixedJoint>();
            currentDrag.transform.position = new Vector3(currentDrag.transform.position.x, currentDrag.transform.position.y + 0.05f, currentDrag.transform.position.z);
            currentDrag.GetComponent<FixedJoint>().connectedBody = gameObject.GetComponent<Rigidbody>();
            currentDrag.GetComponent<Rigidbody>().useGravity = false;


            return;
        }
        if(currentDrag != null)
        {
            handsRig.GetComponent<Rig>().weight = 0;
            Yoke.instance.isGrabbing = false;
            _grabbing = false;
            if(currentDrag.GetComponent<FixedJoint>() != null)
            {
                currentDrag.GetComponent<FixedJoint>().connectedBody = null;
            }
            Destroy(currentDrag.GetComponent<FixedJoint>());
            currentDrag.GetComponent<Rigidbody>().useGravity = true;
        }
    }

    public void OnMove(InputValue value)
    {
        if (!GameMgr.instance.GameRunning) return;
        _moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (!GameMgr.instance.GameRunning) return;
        if (!_grabbing)

        //_characterMovement?.Jump();

        if (_characterMovement.IsGrounded)
        {
            _characterAnimations.Jump();
            StartCoroutine(JumpDelay());
        }

    }

    private void Update()
    {
        // find correct right/forward directions based on main camera rotation
        Vector3 up = Vector3.up;
        Vector3 right = Camera.main.transform.right;
        Vector3 forward = Vector3.Cross(right, up);
        Vector3 moveInput = forward * _moveInput.y + right * _moveInput.x;

        Timer += Time.deltaTime;
        if ((_prevMoveInput != null &&  Timer > cooldown && _prevMoveInput.x != _moveInput.x) ||
            (_prevMoveInput != null && Timer > cooldown && _prevMoveInput.y != _moveInput.y))
            {
            Debug.Log("ROTATE");
            basicAudioPlayer.PlayAudio(yorkScreech);
            Timer = 0.0f;
        }
        _prevMoveInput = _moveInput;
        // send player input to character movement
        _characterMovement.SetMoveInput(moveInput);
        if(!_grabbing)
        _characterMovement.SetLookDirection(moveInput);

        if (_lookInCameraDirection) _characterMovement.SetLookDirection(Camera.main.transform.forward);

        if(!_hasLanded && _characterMovement.IsGrounded && _characterMovement.Velocity.y <= 0)
        {
            Debug.Log("landed");
            _hasLanded = true;
            basicAudioPlayer.PlayAudio(land);
        }
    }

    IEnumerator JumpDelay()
    {
        yield return new WaitForSeconds(jumpDelay);
        _characterMovement?.Jump();
        basicAudioPlayer.PlayAudio(jump);
        _jumpAudio.PlayJumpAudio();
        _hasLanded = false;
        _characterAnimations.Land();
    }
}
