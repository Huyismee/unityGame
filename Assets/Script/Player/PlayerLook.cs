using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Transform playerBody;

    private float mouseSensivity = 150f;

    private float xRotation = 0f;


    public void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * mouseSensivity;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * mouseSensivity;
        //calculate camera rotation for looking up and down
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -80f, 80f);
        //apply this to our camera transform.
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        //Rotate player to look left and right
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
