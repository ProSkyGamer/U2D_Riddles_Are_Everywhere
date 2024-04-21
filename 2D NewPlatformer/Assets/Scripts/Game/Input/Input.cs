using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Input : MonoBehaviour
{
    public static Input Instance { get; private set; }

    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public event EventHandler OnJumpAction;
    public event EventHandler OnInteractAction;
    public event EventHandler OnTestingKeyAction;
    public event EventHandler OnReturnToCheckpointKeyAction;
    public event EventHandler OnChangePlayerAction;
    public event EventHandler OnPauseGameAction;
    public event EventHandler OnSprintAction;
    public event EventHandler OnNextGuideAction;
    public event EventHandler OnPreviousGuideAction;

    public event EventHandler<OnBindingRebingEventArgs> OnBindingRebing;
    public class OnBindingRebingEventArgs : EventArgs
    {
        public Binding bingingChanged;
    }

    private GameInput gameInput;

    public enum Binding
    {
        MoveLeft,
        MoveRight,
        Jump,
        Interact,
        ReturnToCheckpoint,
        ChangePlayer,
        Pause,
        Sprint,
    }

    private void Awake()
    {
        if (Instance != null)
            Destroy(this.gameObject);
        else
            Instance = this;


        gameInput = new GameInput();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            gameInput.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        gameInput.AllBindings.Enable();

        gameInput.AllBindings.Jump.performed += Jump_performed;
        gameInput.AllBindings.Interact.performed += Interact_performed;
        gameInput.AllBindings.TestingKey.performed += TestingKey_performed;
        gameInput.AllBindings.ReturnToCheckpoint.performed += ReturnToCheckpoint_performed;
        gameInput.AllBindings.ChangePlayer.performed += ChangePlayer_performed;
        gameInput.AllBindings.PauseGame.performed += PauseGame_performed;
        gameInput.AllBindings.Sprint.performed += Sprint_performed;
        gameInput.AllBindings.NextGuide.performed += NextGuide_performed;
        gameInput.AllBindings.PreviousGuide.performed += PreviousGuide_performed;
    }

    private void PreviousGuide_performed(InputAction.CallbackContext obj)
    {
        OnPreviousGuideAction?.Invoke(this, EventArgs.Empty);
    }

    private void NextGuide_performed(InputAction.CallbackContext obj)
    {
        OnNextGuideAction?.Invoke(this, EventArgs.Empty);
    }

    private void Sprint_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnSprintAction?.Invoke(this, EventArgs.Empty);
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
        gameInput.AllBindings.Sprint.performed -= Sprint_performed;

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

    public float GetButtonValue(Binding binding)
    {
        switch (binding)
        {
            default:
                return 0;
            case Binding.MoveLeft:
                return gameInput.AllBindings.Movement.ReadValue<Vector2>().x  < 0 ?
                    gameInput.AllBindings.Movement.ReadValue<Vector2>().x : 0;
            case Binding.MoveRight:
                return gameInput.AllBindings.Movement.ReadValue<Vector2>().x > 0 ?
                    gameInput.AllBindings.Movement.ReadValue<Vector2>().x : 0;
            case Binding.Jump:
                return gameInput.AllBindings.Jump.ReadValue<float>();
            case Binding.Interact:
                return gameInput.AllBindings.Interact.ReadValue<float>();
            case Binding.ReturnToCheckpoint:
                return gameInput.AllBindings.ReturnToCheckpoint.ReadValue<float>();
            case Binding.ChangePlayer:
                return gameInput.AllBindings.ChangePlayer.ReadValue<float>();
            case Binding.Pause:
                return gameInput.AllBindings.PauseGame.ReadValue<float>();
            case Binding.Sprint:
                return gameInput.AllBindings.Sprint.ReadValue<float>();
        }
    }

    public bool GetIsSprinting()
    {
        return gameInput.AllBindings.Sprint.ReadValue<float>() > 0;
    }

    public bool IsScrollingMouse(out bool isScrollUp)
    {
        isScrollUp = gameInput.AllBindings.ScrollButtons.ReadValue<Vector2>().y < 0;
        return gameInput.AllBindings.ScrollButtons.ReadValue<Vector2>().y != 0;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.MoveLeft:
                return gameInput.AllBindings.Movement.bindings[1].ToDisplayString();
            case Binding.MoveRight:
                return gameInput.AllBindings.Movement.bindings[2].ToDisplayString();
            case Binding.Jump:
                return gameInput.AllBindings.Jump.bindings[0].ToDisplayString();
            case Binding.Interact:
                return gameInput.AllBindings.Interact.bindings[0].ToDisplayString();
            case Binding.ReturnToCheckpoint:
                return gameInput.AllBindings.ReturnToCheckpoint.bindings[0].ToDisplayString();
            case Binding.ChangePlayer:
                return gameInput.AllBindings.ChangePlayer.bindings[0].ToDisplayString();
            case Binding.Pause:
                return gameInput.AllBindings.PauseGame.bindings[0].ToDisplayString();
            case Binding.Sprint:
                return gameInput.AllBindings.Sprint.bindings[0].ToDisplayString();
        }
    }

    public void RebingBinding(Binding binding, Action onActionRebound)
    {
        gameInput.AllBindings.Disable();

        InputAction inputAction;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.MoveLeft:
                inputAction = gameInput.AllBindings.Movement;
                bindingIndex = 1;
                break;
            case Binding.MoveRight:
                inputAction = gameInput.AllBindings.Movement;
                bindingIndex = 2;
                break;
            case Binding.Jump:
                inputAction = gameInput.AllBindings.Jump;
                bindingIndex = 0;
                break;
            case Binding.Interact:
                inputAction = gameInput.AllBindings.Interact;
                bindingIndex = 0;
                break;
            case Binding.ReturnToCheckpoint:
                inputAction = gameInput.AllBindings.ReturnToCheckpoint;
                bindingIndex = 0;
                break;
            case Binding.ChangePlayer:
                inputAction = gameInput.AllBindings.ChangePlayer;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputAction = gameInput.AllBindings.PauseGame;
                bindingIndex = 0;
                break;
            case Binding.Sprint:
                inputAction = gameInput.AllBindings.Sprint;
                bindingIndex = 0;
                break;
        }
        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                gameInput.AllBindings.Enable();
                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, gameInput.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebing?.Invoke(this, new OnBindingRebingEventArgs()
                {
                    bingingChanged = binding
                });
            }).Start();
    }
}
