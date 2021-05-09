using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 150f;
    [SerializeField] private float sprintSpeed = 200f;
    [SerializeField] private float jumpForce = 100f;
    [Header("Audio")] 
    [SerializeField] private AudioSource stepSource;
    [SerializeField] private List<AudioClip> footStepClips;
    [SerializeField] private float runSoundDelayAdjust = 0.8f;

    private Vector3 _movement;
    private Rigidbody _rb;
    private  PlayerInput _input;

    private bool playingStepSound;
    private bool stepPlayed;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _input = GetComponent<PlayerInput>();
    }

    private void Movement()
    {
        _rb.velocity = new Vector3(_movement.x, _rb.velocity.y, _movement.z);
        
        if (footStepClips != null)
            PlayStepSound();
    }
    
    public void Jump()
    {
        _rb.AddForce(Vector3.up * (jumpForce * Time.deltaTime), ForceMode.Impulse);
    }
    
    public void Walk()
    {
        _movement.x = _input.correctedInput.x * moveSpeed * Time.deltaTime;
        _movement.z = _input.correctedInput.z * moveSpeed * Time.deltaTime;

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
        float delayFinal = Mathf.Abs(_rb.velocity.magnitude) / 10 - runSoundDelayAdjust;
        if (_rb.velocity.magnitude > 0.5f && !playingStepSound)
        {
            playingStepSound = true;
            StartCoroutine(StepDelay(Mathf.Abs(delayFinal)));
        }
    }
    IEnumerator StepDelay(float delay)
    {
        stepSource.PlayOneShot(stepPlayed ? footStepClips[0] : footStepClips[1]);
        //AudioManager.instance.Play(stepPlayed ? footSteps[0] : footSteps[1]);
        stepPlayed = !stepPlayed;
        yield return new WaitForSeconds(delay);
        playingStepSound = false;
    }
}