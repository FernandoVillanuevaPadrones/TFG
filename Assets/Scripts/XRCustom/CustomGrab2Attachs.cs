using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomGrab2Attachs : XRGrabInteractable
{
    [Header("Custom Attachs")]
    [SerializeField] private Transform rightAttach;
    [SerializeField] private Transform leftAttach;


    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);
        if (interactor.transform.name == "RightHand Controller")
        {
            base.attachTransform = rightAttach;
        }
        else if (interactor.transform.name == "LeftHand Controller")
        {
            base.attachTransform = leftAttach;
        }
    }
}
