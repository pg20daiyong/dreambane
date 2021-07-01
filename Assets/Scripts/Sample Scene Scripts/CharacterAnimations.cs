using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimations : MonoBehaviour
{
    // damping time smooths rapidly changing values sent to animator
    [SerializeField] private float _dampTime = 0.1f;
    
    private Animator _animator;
    private CharacterMovement _characterMovement;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _characterMovement = GetComponentInParent<CharacterMovement>();
        _animator.SetBool("IsDead", false);
    }

    private void Update()
    {
        if (!GameMgr.instance.GameRunning) return;

        _animator.SetBool("GameRunning", true);
        // send velocity to animator, ignoring y-velocity
        Vector3 velocity = _characterMovement.Velocity;
        Vector3 flattenedVelocity = new Vector3(velocity.x, 0f, velocity.z);
        float speed = Mathf.Min(_characterMovement.MoveInput.magnitude, flattenedVelocity.magnitude / _characterMovement.Speed);
        _animator.SetFloat("Speed", speed, _dampTime, Time.deltaTime);
        // send grounded state
        _animator.SetBool("IsGrounded", _characterMovement.IsGrounded);
        // send isolated y-velocity
        _animator.SetFloat("VerticalVelocity", velocity.y);
    }

    public void Die()
    {
        print("Died");
        _animator.SetBool("IsDead", true);
        _animator.SetBool("IsGrounded", true);
    }

    public void Jump()
    {
        _animator.SetBool("Jumping", true);
    }

    public void Land()
    {
        _animator.SetBool("Jumping", false);
    }

    public void Win()
    {
        _animator.SetBool("GameWon", true);
    }
}