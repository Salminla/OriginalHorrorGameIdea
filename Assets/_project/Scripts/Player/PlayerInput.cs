using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private Vector3 input;
    public Vector3 correctedInput { get; private set; }

    public void GetInput()
    {
        input.x = Input.GetAxis("Horizontal");
        input.z = Input.GetAxis("Vertical");

        Transform playerTransform = transform;
        var forward = playerTransform.forward;
        var right = playerTransform.right;
        
        correctedInput = (forward * input.z) + (right * input.x);
        correctedInput = Vector3.ClampMagnitude(correctedInput, 1f);
    }
}