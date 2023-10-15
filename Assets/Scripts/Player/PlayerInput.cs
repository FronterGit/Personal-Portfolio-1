using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private InputActionReference onMousePress;
    public enum MouseButton
    {
        Left,
        Right
    }

    private void OnEnable()
    {
        onMousePress.action.performed += OnPlayerInput;
    }

    private void OnDisable()
    {
        onMousePress.action.performed -= OnPlayerInput;
    }

    private void OnPlayerInput(InputAction.CallbackContext obj)
    {
        float mouseInput = obj.ReadValue<float>();  
        if (mouseInput < 0)
        {
            EventBus<MouseInputEvent>.Raise(new MouseInputEvent(MouseButton.Left));
        }
        else
        {
            EventBus<MouseInputEvent>.Raise(new MouseInputEvent(MouseButton.Right));
        }

    }
}
