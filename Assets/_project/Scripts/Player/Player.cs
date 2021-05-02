using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour
{
    public bool enableMovement = true;
    private PlayerMovement _movement;
    private PlayerInput _input;
    private bool jumping;
    private bool sprinting;
    
    private void Start()
    {
        _input = GetComponent<PlayerInput>();
        _movement = GetComponent<PlayerMovement>();
    }
    
    private void Update()
    {
        _input.GetInput();
        
        if (Input.GetKeyDown(KeyCode.Space))
            jumping = true;
        
        sprinting = Input.GetKey(KeyCode.LeftShift);
    }

    private void FixedUpdate()
    {
        if (!enableMovement) return;
        _movement.Walk();

        if (jumping)
        {
            jumping = false;
            _movement.Jump();
        }

        if (sprinting)
        {
            _movement.Sprint();
        }
    }
}