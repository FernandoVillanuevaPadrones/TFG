
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;
using System;

public class PlayerMapMovement : MonoBehaviour
{
    [SerializeField] private InputActionProperty moveInput;
    [SerializeField] private InputActionProperty turnInput;
    [SerializeField] private float moveSpeedRoom = 1e-05f;
    [SerializeField] private float moveSpeedDoor = 1e-04f;
    [SerializeField] private float angleCameraOffset;
    private GameObject cameraObject;
    private GameObject XROriginGB;

    private GameObject[] mapsRoomsToMove = new GameObject[2];
    private GameObject HUDGB;
    private GameObject hudRotateOffsetGB;

    private float currentSpeedOffset;

    private void Awake()
    {
        XROriginGB = GameObject.Find("XR Origin");
        cameraObject = XROriginGB.transform.Find("Camera Offset/Main Camera").gameObject;
        currentSpeedOffset = moveSpeedRoom;
        
    }

    private void FixedUpdate()
    {

       
        transform.localScale = HUDGB.transform.localScale;

    }

    public void GetRoomsToMove(GameObject rooms, GameObject doors)
    {
        // Map/HUDRotateOffset/HUDMoveOffset/HUD/RoomsMap
        // Esto esta en HUD asi que cogemos Map
        HUDGB = rooms.transform.parent.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("RoomMap"))
        {
            currentSpeedOffset = moveSpeedRoom;
        }
        else if (other.transform.CompareTag("DoorMap"))
        {
            currentSpeedOffset = moveSpeedDoor;
        }
    }

}
