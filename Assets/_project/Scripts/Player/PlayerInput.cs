using UnityEngine;
using Valve.VR;

public class PlayerInput : MonoBehaviour
{
    SteamVR_Action_Vector2 moveVector;
    public SteamVR_Action_Boolean sprintBool;
    
    private Vector3 input;
    public Vector3 correctedInput { get; private set; }

    private void Start()
    {
        moveVector = SteamVR_Input.GetAction<SteamVR_Action_Vector2>("Move");
        sprintBool = SteamVR_Input.GetAction<SteamVR_Action_Boolean>("Sprint");
    }

    public void GetInput()
    {
        if (GameManager.instance != null && GameManager.instance.VRActive)
        {
            input.x = moveVector.axis.x;
            input.z = moveVector.axis.y;
        }
        else
        {
            input.x = Input.GetAxis("Horizontal");
            input.z = Input.GetAxis("Vertical");
        }
        

        Transform playerTransform = transform;
        var forward = playerTransform.forward;
        var right = playerTransform.right;
        
        correctedInput = (forward * input.z) + (right * input.x);
        correctedInput = Vector3.ClampMagnitude(correctedInput, 1f);
    }
}