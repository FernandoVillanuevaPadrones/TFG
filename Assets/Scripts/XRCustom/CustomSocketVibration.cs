using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CustomSocketVibration : XRSocketInteractor
{
    [Header("Vibration Settings")]
    [SerializeField] private float amplitude;
    [SerializeField] private float duration;
    
    [Header("Attach Points")]
    [SerializeField] private Transform rightAttach;
    [SerializeField] private Transform leftAttach;


    protected override void OnHoverEntered(XRBaseInteractable interactable)
    {

        if (interactable.selectingInteractor.transform.name == "RightHand Controller")
        {
            base.attachTransform = rightAttach;
            Vibrate(interactable.selectingInteractor);
        }
        else if (interactable.selectingInteractor.transform.name == "LeftHand Controller")
        {
            base.attachTransform = leftAttach;
            Vibrate(interactable.selectingInteractor);
        }
    }

    protected override void OnSelectExiting(XRBaseInteractable interactable)
    {
        base.OnSelectExiting(interactable);

    }

    

    private void Vibrate(XRBaseInteractor interactor)
    {
        
        interactor.GetComponent<ActionBasedController>().SendHapticImpulse(0.5f, 0.25f);
    }
}
