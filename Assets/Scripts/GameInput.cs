using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using System;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEngine.InputSystem;

public class GameInput : MonoBehaviour
{
    public const string PLAYER_PREFS_REBIND = "Player Prefs Rebind";

    public event EventHandler OnInteractAction;
    public event EventHandler OnInteractAlternativeAction;
    public event EventHandler OnTogglePauseGame; 
    public event EventHandler OnRebindBinding;

    private PlayerInputActions playerInputActions;

    public enum Binding
    {
        Move_Up, 
        Move_Down, 
        Move_Left, 
        Move_Right,
        Interact,
        Interact_Alternate,
        Pause,
    }
    public static GameInput Instance { get; private set; }
    
    private  void Awake()
    {
        Instance = this;

        playerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_REBIND))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_REBIND));
        }

        playerInputActions.Player.Enable();

        playerInputActions.Player.Interact.performed += Interact_performed;
        playerInputActions.Player.InteractAlternate.performed += InteractAlternative_performed;
        playerInputActions.Player.Pause.performed += Pause_performed; 
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternative_performed;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Dispose();
    }
    private void Pause_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnTogglePauseGame?.Invoke(this, EventArgs.Empty);
    }

    private void InteractAlternative_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAlternativeAction?.Invoke(this, EventArgs.Empty);
    }

    private void Interact_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }

    public Vector2 GetMovementVectorNormalized()
    {
        if(!GameHandler.Instance.CanMove()) { return Vector2.zero; }

        Vector2 inputVector = playerInputActions.Player.Move.ReadValue<Vector2>();
        inputVector = inputVector.normalized;
        return inputVector;
    }

    public string GetBindingText(Binding binding)
    {
        switch (binding)
        {
            default:
            case Binding.Move_Up:
                return playerInputActions.Player.Move.bindings[1].ToDisplayString();
            case Binding.Move_Down:
                return playerInputActions.Player.Move.bindings[2].ToDisplayString();
            case Binding.Move_Left:
                return playerInputActions.Player.Move.bindings[3].ToDisplayString();
            case Binding.Move_Right:
                return playerInputActions.Player.Move.bindings[4].ToDisplayString();
            case Binding.Interact:
                return playerInputActions.Player.Interact.bindings[0].ToDisplayString();
            case Binding.Interact_Alternate:
                return playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString();
            case Binding.Pause:
                return playerInputActions.Player.Pause.bindings[0].ToDisplayString();
        }
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        playerInputActions.Disable();

        InputAction inputActions;
        int rebindingIndex;

        switch (binding){
            default:
            case Binding.Move_Up:
                inputActions = playerInputActions.Player.Move;
                rebindingIndex = 1;
                break;
            case Binding.Move_Down:
                inputActions = playerInputActions.Player.Move;
                rebindingIndex = 2;
                break;
            case Binding.Move_Left:
                inputActions = playerInputActions.Player.Move;
                rebindingIndex = 3;
                break;
            case Binding.Move_Right:
                inputActions = playerInputActions.Player.Move;
                rebindingIndex = 4;
                break;
            case Binding.Interact:
                inputActions = playerInputActions.Player.Interact;
                rebindingIndex = 0;
                break;
            case Binding.Interact_Alternate:
                inputActions = playerInputActions.Player.InteractAlternate;
                rebindingIndex = 0;
                break;
            case Binding.Pause:
                inputActions = playerInputActions.Player.Pause;
                rebindingIndex = 0;
                break;
        }

        inputActions.PerformInteractiveRebinding(rebindingIndex)
            .OnComplete(callback =>
            {
                callback.Dispose();
                playerInputActions.Enable();
                onActionRebound();
                playerInputActions.SaveBindingOverridesAsJson();

                PlayerPrefs.SetString(PLAYER_PREFS_REBIND, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnRebindBinding?.Invoke(this, EventArgs.Empty);
            })
            .Start();
    }
}
