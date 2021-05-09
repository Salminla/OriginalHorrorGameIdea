using System;
using UnityEngine;

public class LookWithMouse : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    float _xRotation = 0f;
    
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        mouseSensitivity = PlayerPrefs.GetInt("sensitivity", Convert.ToInt32(mouseSensitivity));
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(_xRotation, 0f, 0f);

        playerBody.Rotate(Vector3.up * mouseX);
    }
}
