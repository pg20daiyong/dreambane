using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
#pragma warning disable 649

// 3D implentation 
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(NavMeshAgent))]
public class CharacterMovement3D : CharacterMovement
{
    // public-read private-set properties
    public override Vector3 Velocity { get => _rigidbody.velocity; protected set => _rigidbody.velocity = value; }
    [SerializeField] private CharacterBalance _balacing;

    // private fields
    private Rigidbody _rigidbody;
    private NavMeshAgent _navMeshAgent;

    private float fallingForce = 0;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        _rigidbody.useGravity = false;
        
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.updatePosition = false;
        _navMeshAgent.updateRotation = false;
        
        LookDirection = transform.forward;
    }

    public override void SetMoveInput(Vector3 input)
    {
        if (!GameMgr.instance.GameRunning) return;
        Vector3 flattened = new Vector3(input.x, 0f, input.z);
        base.SetMoveInput(flattened);
    }

    public void MoveTo(Vector3 destination)
    {
        _navMeshAgent.SetDestination(destination);
    }

    public void Stop()
    {
        _navMeshAgent.ResetPath();
        SetMoveInput(Vector3.zero);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        // sends correct forward/right inputs to GetMovementAcceleration and applies result to rigidbody
        Vector3 input = MoveInput;
        if (!GameMgr.instance.GameRunning) input = Vector3.zero;
        if (ForcedMovement > 0f) input = transform.forward * ForcedMovement;
        Vector3 right = Vector3.Cross(transform.up, input);
        Vector3 forward = Vector3.Cross(right, GroundNormal);
        _rigidbody.AddForce(GetMovementAcceleration(forward));
    }

    protected override void Update()
    {
        base.Update();

        // overrides current input with pathing direction if MoveTo has been called
        if(_navMeshAgent.hasPath)
        {
            Vector3 nextPathPoint = _navMeshAgent.path.corners[1];
            Vector3 pathDir = (nextPathPoint - transform.position).normalized;
            SetMoveInput(pathDir);
            SetLookDirection(pathDir);
        }



        // syncs navmeshagent position with character position
        _navMeshAgent.nextPosition = transform.position;

        if (!IsGrounded && _rigidbody.velocity.y < 0)
        {
            fallingForce += -_gravity * Time.deltaTime;
            return;
        }
        _balacing.AddFallingForce(fallingForce);
        fallingForce = 0;
    }

    protected override bool CheckGrounded()
    {
        // raycast to find ground
        bool hit = Physics.Raycast(_groundCheckStart, -transform.up, out RaycastHit hitInfo, _groundCheckDistance, _groundMask);
        // gets velocity of surface underneath character if applicable
        if (hit && hitInfo.rigidbody != null) SurfaceVelocity = hitInfo.rigidbody.velocity;
        else SurfaceVelocity = Vector3.zero;
        // sends hit info for grounding resolution
        return ResolveGrounded(hit, hitInfo.normal);
    }

    public void DisableTurnControl()
    {
        _controlRotation = false;
    }


}