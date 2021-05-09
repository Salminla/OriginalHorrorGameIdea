using System;
using UnityEngine;
using Valve.VR;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    [field: SerializeField] public float SprintStrength { get; private set; } = 100;
    [SerializeField] private float sprintRecoverySpeed = 0.5f;
    [SerializeField] private float sprintUseSpeed = 0.5f;
    public bool enableMovement = true;
    public SteamVR_Action_Boolean sprintBool;
    private PlayerMovement _movement;
    private PlayerInput _input;
    private Camera mainCamera;
    private bool jumping;
    private bool holdSprintKey;
    private bool allowSprint = true;
    private bool sprinting;
    
    private void Start()
    {
        _input = GetComponent<PlayerInput>();
        _movement = GetComponent<PlayerMovement>();
        mainCamera = Camera.main;

        mainCamera.fieldOfView = PlayerPrefs.GetInt("fov", Convert.ToInt32(mainCamera.fieldOfView));
    }
    
    private void Update()
    {
        _input.GetInput();
        
        if (Input.GetKeyDown(KeyCode.Space) && GameManager.instance.allowJump)
            jumping = true;

        holdSprintKey = !GameManager.instance.VRActive ? Input.GetKey(KeyCode.LeftShift) : _input.sprintBool.state;

        GameManager.instance.sprintStrength = SprintStrength;
    }

    private void FixedUpdate()
    {
        if (!enableMovement) return;
        if (!sprinting)
            _movement.Walk();
        if (jumping)
        {
            jumping = false;
            _movement.Jump();
        }
        SprintStates();
    }

    void SprintStates()
    {
        SprintStrength = Mathf.Clamp(SprintStrength, 0f, 100f);
        if (holdSprintKey && allowSprint)
        {
            sprinting = true;
            _movement.Sprint();
            SprintStrength -= sprintUseSpeed;
        }

        if (SprintStrength < 1f)
            allowSprint = false;

        if (holdSprintKey && allowSprint) return;
        sprinting = false;
        SprintStrength += sprintRecoverySpeed;
        if (SprintStrength > 50f)
            allowSprint = true;
    }
}