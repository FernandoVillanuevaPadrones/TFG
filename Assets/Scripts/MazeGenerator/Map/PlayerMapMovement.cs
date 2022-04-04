using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

public class PlayerMapMovement : MonoBehaviour
{
    [SerializeField] private InputActionProperty moveInput;
    [SerializeField] private float moveSpeedOffset;

    
    private void FixedUpdate()
    {
        var leftHandValue = moveInput.action?.ReadValue<Vector2>() ?? Vector2.zero;

        Vector3 move = new Vector3(leftHandValue.x, leftHandValue.y, 0);

        transform.position += move * moveSpeedOffset;
        
    }

}
