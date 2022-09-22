using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // VARIABLES
    [SerializeField] private float mouseSensitivity;


    // REFERENCES
    private Transform parent;   // The player

    private void Start()
    {
        parent = transform.parent; // The parent of the camera is the object we want to rotate.
        Cursor.lockState = CursorLockMode.Locked; // If we want to lock the mouse to the middle of the screen. For testing, click escape to see it again.
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; // Mouse X is also in Input Manager.

        parent.Rotate(Vector3.up, mouseX);  // We rotate around the "up-axis" or the "y-axis". We rotate it mouseX amount.
    }

}
