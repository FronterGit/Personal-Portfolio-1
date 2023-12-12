using System;
using System.Collections;
using System.Collections.Generic;
using EventBus;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private LayerMask UILayerMask;
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
        var mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
        
        // var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // bool hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, UILayerMask);
        
        float mouseInput = obj.ReadValue<float>();
        if (mouseInput < 0)
        {
            if(EventSystem.current.IsPointerOverGameObject()) return;
            EventBus<MouseInputEvent>.Raise(new MouseInputEvent(MouseButton.Left, mousePos));
        }
        else
        {
            EventBus<MouseInputEvent>.Raise(new MouseInputEvent(MouseButton.Right, mousePos));
        }

    }

    private void Update()
    {
        //Cheats
        if (Keyboard.current[Key.W].wasPressedThisFrame)
        {
            EventBus<LevelCompleteEvent>.Raise(new LevelCompleteEvent(true));
        }
    }
}
