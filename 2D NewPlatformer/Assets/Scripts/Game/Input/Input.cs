using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input : MonoBehaviour
{
    public static Input Instance { get; private set; }

    public event EventHandler OnJumpAction;
    public event EventHandler OnInteractAction;
    public event EventHandler OnTestingKeyAction;
    public event EventHandler OnReturnToCheckpointKeyAction;
    public event EventHandler OnChangePlayerAction;
    public event EventHandler OnPauseGameAction;

    private GameInput gameInput;

    private void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;


        gameInput = new GameInput();

        gameInput.AllBindings.Enable();

        gameInput.AllBindings.Jump.performed += Jump_performed;
        gameInput.AllBindings.Interact.performed += Interact_performed;
        gameInput.AllBindings.TestingKey.performed += TestingKey_performed;
        gameInput.AllBindings.ReturnToCheckpoint.performed += ReturnToCheckpoint_performed;
        gameInput.AllBindings.ChangePlayer.performed += ChangePlayer_performed;
        gameInput.AllBindings.PauseGame.performed += PauseGame_performed;

    }

    private void PauseGame_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseGameAction?.Invoke(this, EventArgs.Empty);
    }

    private void ChangePlayer_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnChangePlayerAction?.Invoke(this, EventArgs.Empty);
    }

    private void ReturnToCheckpoint_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnReturnToCheckpointKeyAction?.Invoke(this, EventArgs.Empty);
    }

    private void TestingKey_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnTestingKeyAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    private void OnDestroy()
    {
        gameInput.AllBindings.Jump.performed -= Jump_performed;
        gameInput.AllBindings.Interact.performed -= Interact_performed;
        gameInput.AllBindings.TestingKey.performed -= TestingKey_performed;
        gameInput.AllBindings.ReturnToCheckpoint.performed -= ReturnToCheckpoint_performed;
        gameInput.AllBindings.ChangePlayer.performed -= ChangePlayer_performed;
        gameInput.AllBindings.PauseGame.performed -= PauseGame_performed;

        gameInput.Dispose();
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnJumpAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVector()
    {
        Vector2 inputVector = gameInput.AllBindings.Movement.ReadValue<Vector2>();

        return inputVector;
    }
}
