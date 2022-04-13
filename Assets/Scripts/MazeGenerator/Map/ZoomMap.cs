using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class ZoomMap : MonoBehaviour
{
    [SerializeField] private float mapScaleSpeed;
    [SerializeField] private float minScale = 1f;
    [SerializeField] private float maxScale = 4f;
    [SerializeField] private InputActionReference zoomInButton;
    [SerializeField] private InputActionReference zoomOutButton;

    private OffsetGrab mapGrabScript;
    private bool canZoom = false;
    private bool doingZoom;

    // Start is called before the first frame update
    void Start()
    {
        // Map/HUDMoveOffset/HUDCameraOffset/HUD
        // Esto esta en HUD asi que cogemos Map
        mapGrabScript = transform.parent.parent.parent.GetComponent<OffsetGrab>();

        mapGrabScript.onSelectEntered.AddListener(MapGrabbed);
        mapGrabScript.onSelectExited.AddListener(MapReleased);

       
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
}
