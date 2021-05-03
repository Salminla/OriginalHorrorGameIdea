using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 150f;
    [SerializeField] private float sprintSpeed = 200f;
    [SerializeField] private float jumpForce = 100f;
    [SerializeField] private List<AudioClip> footSteps;
    
    private Vector3 _movement;
    private Rigidbody _rb;
    private  PlayerInput _input;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInput>();
    }

    private void Movement()
    {
        _rb.velocity = new Vector3(_movement.x, _rb.velocity.y, _movement.z);
    }
    
    public void Jump()
    {
        _rb.AddForce(Vector3.up * jumpForce * Time.deltaTime, ForceMode.Impulse);
    }
    
    public void Walk()
    {
        _movement.x = _input.correctedInput.x * moveSpeed * Time.deltaTime;
        _movement.z = _input.correctedInput.z * moveSpeed * Time.deltaTime;
        //PlayStepSound();
        Movement();
    }
    
    public void Sprint()
    {
        _movement.x = _input.correctedInput.x * sprintSpeed * Time.deltaTime;
        _movement.z = _input.correctedInput.z * sprintSpeed * Time.deltaTime;

        Movement();
    }

    void PlayStepSound()
    {
        if ((Mathf.Abs(_rb.velocity.x) > 1f || Mathf.Abs(_rb.velocity.z) > 1f) && !AudioManager.instance.effectsSource.isPlaying)
        {
            AudioManager.instance.Play(footSteps[Random.Range(0,2)]);
        }
    }
}