using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ZoomMap : MonoBehaviour
{

    [SerializeField] private float moveSpaceX = 0.02f;
    [SerializeField] private float moveSpaceY = 0.02f;

    [SerializeField] private float mapScaleSpeed;
    [SerializeField] private float minScale = 1f;
    [SerializeField] private float maxScale = 4f;
    [SerializeField] private InputActionReference zoomInButton;
    [SerializeField] private InputActionReference zoomOutButton;

    private OffsetGrab mapGrabScript;
    private bool canZoom = false;
    private bool doingZoom;

    private int lastRoomI = 0;
    private int lastRoomJ = 0;


    // Start is called before the first frame update
    void Start()
    {
        // Map/HUDMoveOffset/HUDCameraOffset/HUD
        // Esto esta en HUD asi que cogemos Map
        mapGrabScript = transform.parent.parent.parent.GetComponent<OffsetGrab>();

        mapGrabScript.onSelectEntered.AddListener(MapGrabbed);
        mapGrabScript.onSelectExited.AddListener(MapReleased);

        lastRoomI = 0;
        lastRoomJ = 0;


}
    private void MapReleased(XRBaseInteractor arg0)
    {
            canZoom = false;      
    }

    private void MapGrabbed(XRBaseInteractor arg0)
    {
        if (arg0.name == "LeftHand Controller" || arg0.name == "RightHand Controller")
        {
            canZoom = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canZoom)
        {
            if (zoomInButton.action.ReadValue<float>() != 0 && !doingZoom)
            {

                doingZoom = true;
                StartCoroutine(ZoomIn());
            }
            else if (zoomOutButton.action.ReadValue<float>() != 0 && !doingZoom)
            {
                doingZoom = true;
                StartCoroutine(ZoomOut());
            }
            else if (zoomInButton.action.ReadValue<float>() == 0 && zoomOutButton.action.ReadValue<float>() == 0)
            {
                doingZoom = false;
            }
            
        }
    }

    private IEnumerator ZoomOut()
    {
        while (transform.parent.localScale.x > minScale && doingZoom)
        {
            transform.parent.localScale -= new Vector3(mapScaleSpeed, mapScaleSpeed, 0f);
            yield return new WaitForSeconds(0);
        }

        yield return null;
    }

    private IEnumerator ZoomIn()
    {
        while (transform.parent.localScale.x < maxScale && doingZoom)
        {
            transform.parent.localScale += new Vector3(mapScaleSpeed, mapScaleSpeed, 0f);
            yield return new WaitForSeconds(0);
        }

        yield return null;
    }

    internal void MoveMapTo(int newRoomI, int newRoomJ)
    {
        //this happens at the start
        if (lastRoomI == 0 && lastRoomJ == 0)
        {
            lastRoomI = newRoomI;
            lastRoomJ = newRoomJ;
            return;
        }

        // Antigua sala esta a la izquierda, mover hacia la izq
        if (newRoomI == (lastRoomI + 2) && newRoomJ == lastRoomJ)
        {
            transform.localPosition -= new Vector3(moveSpaceX, 0f, 0f);
        }
        // Antigua sala esta a la derecha, mover hacia la der
        else if (newRoomI == (lastRoomI - 2) && newRoomJ == lastRoomJ)
        {
            transform.localPosition += new Vector3(moveSpaceX, 0f, 0f);
        }
        // Antigua Sala esta abajo, movemos hacia abajo
        else if (newRoomJ == (lastRoomJ + 2) && newRoomI == lastRoomI )
        {
            transform.localPosition -= new Vector3(0f, moveSpaceY, 0f);

        }
        // si la de abajo tiene sala quito pared sino se pone , pero la puerta se quita siempre
        else if (newRoomJ == (lastRoomJ - 2) && newRoomI == lastRoomI)
        {
            transform.localPosition += new Vector3(0f, moveSpaceY, 0f);
        }

        lastRoomI = newRoomI;
        lastRoomJ = newRoomJ;
    }
}
