using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 playerVelocity = Vector3.zero;
    private Vector3 jumpDirection;
    private bool isGrounded;
    private bool lerpCrouch, crouching, sprinting;
    private bool jumping = false;
    public float speed = 5f;
    public float gravity = -25f;
    public float jumpHeight = 0.3f;
    public float crouchTimer;
    private bool isMoving;
    private Vector3 lastPosition =new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = controller.isGrounded;
        if (lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1;
            p *= p;
            if (crouching)
                controller.height = Mathf.Lerp(controller.height, 1, p);
            else
            {
                controller.height = Mathf.Lerp(controller.height, 2, p);
            }

            if (p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0;
            }
        }
        Debug.Log(crouchTimer);
    }

    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        controller.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);
        playerVelocity.y += gravity * Time.deltaTime;
        if (isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
            playerVelocity.x = 0;
            playerVelocity.z = 0;
            jumpDirection.x = input.x;
            jumpDirection.z = input.y;
            if (jumping)
                jumping = Jumped();
        }
        controller.Move(transform.TransformDirection(playerVelocity) * Time.deltaTime);
        //Debug.Log(playerVelocity.y);

        if (lastPosition != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
            //for further use
        }
        else
        {
            isMoving = false;
            //for further use
        }
        lastPosition = gameObject.transform.position;
    }

    public void Jump()
    {
        if (isGrounded && !crouching)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            playerVelocity.x = jumpDirection.x * 1f * speed;
            playerVelocity.z = jumpDirection.z * 1f * speed;
            jumpDirection = Vector3.zero;
            speed = 0;
            jumping = true;
        }
    }














    public bool Jumped()
    {
        if (sprinting)
            speed = 8;
        else
            speed = 5;
        return false;
    }

    public void Crouch()
    {
        crouching = !crouching;
        crouchTimer = 0;
        lerpCrouch = true;
        if (crouching)
            speed = 3;
        else
        {
            speed = 5;
        }
    }

    public void Sprint()
    {
        sprinting = !sprinting;
        if (sprinting && !crouching)
            speed = 8;
        else if (crouching)
            speed = 3;
        else
        {
            speed = 5;
        }
    }
}
