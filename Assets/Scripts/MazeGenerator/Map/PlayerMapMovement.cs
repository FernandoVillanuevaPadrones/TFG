
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
    private GameObject hudMoveOffsetGB;
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

        // Parent para seguir al cuerpo en caso de que rote con el mando
        Vector3 eulerRotation = new Vector3(transform.parent.localEulerAngles.x, transform.parent.localEulerAngles.y, XROriginGB.transform.localEulerAngles.y);
        transform.parent.localRotation = Quaternion.Euler(-eulerRotation);

        /*
        // Rotar en funcion de la camara
        eulerRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, cameraObject.transform.localEulerAngles.y + angleCameraOffset);
        transform.localRotation = Quaternion.Euler(-eulerRotation);
        */

        var leftHandValue = moveInput.action?.ReadValue<Vector2>() ?? Vector2.zero;
        Vector3 move = new Vector3(leftHandValue.x, leftHandValue.y, 0);

        //transform.localPosition += move * moveSpeedOffset;

        

        transform.localScale = hudMoveOffsetGB.transform.localScale;

        /*
        if (mapsRoomsToMove.Length != 0)
        {
            eulerRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, cameraObject.transform.localEulerAngles.y + angleCameraOffset);
            hudCameraOffsetGB.transform.localRotation = Quaternion.Euler(eulerRotation);

            foreach (var item in mapsRoomsToMove)
            {
                item.transform.position -= move * currentSpeedOffset;

                Debug.Log(item.transform.up);
                
            }
        }*/

        eulerRotation = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, cameraObject.transform.localEulerAngles.y + angleCameraOffset);
        hudRotateOffsetGB.transform.localRotation = Quaternion.Euler(eulerRotation);




        hudMoveOffsetGB.transform.localPosition -= move * currentSpeedOffset;
        Debug.Log(hudRotateOffsetGB);
        Debug.Log(hudRotateOffsetGB.transform.localPosition);
        //Debug.Log(move);

    }

    public void GetRoomsToMove(GameObject rooms, GameObject doors)
    {
        mapsRoomsToMove[0] = rooms;
        mapsRoomsToMove[1] = doors;
       
        // Map/HUDRotateOffset/HUDMoveOffset/HUD/RoomsMap
        // Esto esta en HUD asi que cogemos Map
        hudMoveOffsetGB = rooms.transform.parent.parent.gameObject;
        hudRotateOffsetGB = hudMoveOffsetGB.transform.parent.gameObject;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("RoomMap"))
        {
            Debug.Log("Cambia a Room");
            currentSpeedOffset = moveSpeedRoom;
        }
        else if (other.transform.CompareTag("DoorMap"))
        {
            Debug.Log("Cambia a Door");
            currentSpeedOffset = moveSpeedDoor;
        }
    }

}
