using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    #region EXPOSED_FIELDS
    [SerializeField] private float normalSpeed = 5f;
    [SerializeField] private float sprintSpeed = 8f;
    [SerializeField] private float gravity = -9.8f;
    [SerializeField] private float jumpHeight = 1f;
    #endregion

    #region PRIVATE_FIELDS
    private CharacterController cc = null;
    private Vector3 playerVelocity = new();

    private float speed = 5f;

    private bool isGrounded = false;
    private bool isSprinting = false;
    private bool isCrouching = false;
    private bool lerpCrouch = false;

    private float crouchTimer = 0f;
    #endregion

    #region UNITY_CALLS
    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        isGrounded = cc.isGrounded;

        if(lerpCrouch)
        {
            crouchTimer += Time.deltaTime;
            float p = crouchTimer / 1f;
            p *= p;
            if(isCrouching)
            {
                cc.height = Mathf.Lerp(cc.height, 1f, p);
            }
            else
            {
                cc.height = Mathf.Lerp(cc.height, 2f, p);
            }

            if(p > 1)
            {
                lerpCrouch = false;
                crouchTimer = 0f;
            }
        }
    }
    #endregion

    #region PUBLIC_METHODS
    public void ProcessMove(Vector2 input)
    {
        Vector3 moveDirection = Vector3.zero;
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        cc.Move(transform.TransformDirection(moveDirection) * speed * Time.deltaTime);

        playerVelocity.y += gravity * Time.deltaTime;

        if(isGrounded && playerVelocity.y < 0f)
        {
            playerVelocity.y = -2f;
        }

        cc.Move(playerVelocity * Time.deltaTime);
    }

    public void Jump()
    {
        if(isGrounded)
        {
            playerVelocity.y = Mathf.Sqrt(jumpHeight * -3f * gravity);
        }
    }

    public void Sprint()
    {
        isSprinting = !isSprinting;

        if (isSprinting)
        {
            speed = sprintSpeed;
        }
        else
        {
            speed = normalSpeed;
        }
    }

    public void Crouch()
    {
        isCrouching = !isCrouching;
        crouchTimer = 0f;
        lerpCrouch = true;
    }
    #endregion
}
