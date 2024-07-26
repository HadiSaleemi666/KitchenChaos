using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    private const string PLAYER_PREFS_BINDINGS = "InputBindings";

    public static GameInput Instance { get; private set; }

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternateAction;
    public event EventHandler OnPauseGame;
    public event EventHandler OnBindingRebind;

    private PlayerInputControl playerInputControl;

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
        GamePad_Interact,
        GamePad_InteractAlternate,
        GamePad_Pause,
    }

    private void Awake()
    {
        playerInputControl = new PlayerInputControl();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDINGS))
        {
            playerInputControl.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDINGS));
        }

        playerInputControl.Player.Enable();

        playerInputControl.Player.Interact.performed += Interact_performed;
        playerInputControl.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputControl.Player.Pause.performed += Pause_performed;

        Instance = this;

        
    }

    private void OnDestroy()
    {
        playerInputControl.Player.Interact.performed -= Interact_performed;
        playerInputControl.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputControl.Player.Pause.performed -= Pause_performed;

        playerInputControl.Dispose();
    }

    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnPauseGame?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternate_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternateAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        Vector2 inputVector = playerInputControl.Player.Move.ReadValue<Vector2>();

        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return playerInputControl.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputControl.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputControl.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputControl.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputControl.Player.Interact.bindings[0].ToDisplayString();
            case Binding.InteractAlternate:
                return playerInputControl.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputControl.Player.Pause.bindings[0].ToDisplayString();
            case Binding.GamePad_Interact:
                return playerInputControl.Player.Interact.bindings[1].ToDisplayString();
            case Binding.GamePad_InteractAlternate:
                return playerInputControl.Player.InteractAlternate.bindings[1].ToDisplayString();
            case Binding.GamePad_Pause:
                return playerInputControl.Player.Pause.bindings[1].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        playerInputControl.Player.Disable();

        InputAction inputActions;
        int bindingIndex;

        switch (binding)
        {
            default:
            case Binding.Move_Up:
                inputActions = playerInputControl.Player.Move;
                bindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputActions = playerInputControl.Player.Move;
                bindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputActions = playerInputControl.Player.Move;
                bindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputActions = playerInputControl.Player.Move;
                bindingIndex = 4;
                break;
            case Binding.Interact:
                inputActions = playerInputControl.Player.Interact;
                bindingIndex = 0;
                break;
            case Binding.InteractAlternate:
                inputActions = playerInputControl.Player.InteractAlternate;
                bindingIndex = 0;
                break;
            case Binding.Pause:
                inputActions = playerInputControl.Player.Pause;
                bindingIndex = 0;
                break;
            case Binding.GamePad_Interact:
                inputActions = playerInputControl.Player.Interact;
                bindingIndex = 1;
                break;
            case Binding.GamePad_InteractAlternate:
                inputActions = playerInputControl.Player.InteractAlternate;
                bindingIndex = 1;
                break;
            case Binding.GamePad_Pause:
                inputActions = playerInputControl.Player.Pause;
                bindingIndex = 1;
                break;
        }

        
        inputActions.PerformInteractiveRebinding(bindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                playerInputControl.Player.Enable();
                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDINGS, playerInputControl.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke(this, EventArgs.Empty);
            })
            .Start();

    }
}