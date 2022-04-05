
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerMapMovement : MonoBehaviour
{
    [SerializeField] private InputActionProperty moveInput;
    [SerializeField] private float moveSpeedOffset;
    private GameObject cameraObject;

    private void Awake()
    {
        cameraObject = GameObject.Find("XR Origin/Camera Offset/Main Camera");
    }

    private void FixedUpdate()
    {
        
        transform.Rotate(0, 0, 0);

        var leftHandValue = moveInput.action?.ReadValue<Vector2>() ?? Vector2.zero;

        Vector3 move = new Vector3(leftHandValue.x, leftHandValue.y, 0);

        transform.localPosition += move * moveSpeedOffset;
        
    }

}
