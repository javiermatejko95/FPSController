using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region PRIVATE_FIELDS
    private PlayerInput playerInput = null;
    private PlayerInput.PlayerActions playerActions;

    private PlayerController playerController = null;
    private PlayerLook playerLook = null;
    #endregion

    #region UNITY_CALLS
    private void Awake()
    {
        playerInput = new PlayerInput();
        playerActions = playerInput.Player;
        playerController = GetComponent<PlayerController>();
        playerLook = GetComponent<PlayerLook>();

        playerActions.Jump.performed += ctx => playerController.Jump();
        playerActions.Sprint.performed += ctx => playerController.Sprint();
        playerActions.Crouch.performed += ctx => playerController.Crouch();
    }

    private void FixedUpdate()
    {
        playerController.ProcessMove(playerActions.Move.ReadValue<Vector2>());
    }

    private void Update()
    {
        playerLook.ProcessLook(Mouse.current.delta.ReadValue());
    }

    private void OnEnable()
    {
        playerActions.Enable();
    }

    private void OnDisable()
    {
        playerActions.Disable();
    }
    #endregion
}
