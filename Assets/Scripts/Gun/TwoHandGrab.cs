using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TwoHandGrab : XRGrabInteractable
{
    [Header("Second Hand Grab Interactable")]
    [SerializeField] private XRSimpleInteractable secondHandGrab;
    private XRBaseInteractor secondInteractor;
    public Vector3 offset;

    [Header("Prefabs Hands")]
    [SerializeField] private GameObject rightHandPlayerGB;
    [SerializeField] private GameObject leftHandPlayerGB;
    [SerializeField] private GameObject rightHandMainObjectGB;
    [SerializeField] private GameObject leftHandMainObjectGB;
    [SerializeField] private GameObject rightHandSecondGunGB;
    [SerializeField] private GameObject leftHandSecondGunGB;
    [SerializeField] private enum TwoHandRotationType { None, First, Second}
    [Header("Type Of Rotation")]
    [SerializeField] private TwoHandRotationType twoHandRotationType;

    [Header("Attach points")]
    [SerializeField] private Transform rightAttachTransform;
    [SerializeField] private Transform leftAttachTransform;



    private Quaternion initialRightRotation;
    private Quaternion initialLeftRotation;
    private string lastMainSelector = "";
    private string lastSecondSelector = "";

    
    private void Start()
    {
        secondHandGrab.onSelectEntered.AddListener(OnSecondHandGrab);
        secondHandGrab.onSelectExited.AddListener(OnSecondHandRelease);

        // Store the initial rotation of the attach point to reset it when grabbed by one hand
        initialRightRotation = rightAttachTransform.localRotation;
        initialLeftRotation = leftAttachTransform.localRotation;
    }

    private void OnSecondHandRelease(XRBaseInteractor arg0)
    {
        secondInteractor = null;

        // When the second hand is released and we are still grabbing the hand, we reset the position of the gun
        if (selectingInteractor != null)
        {
            rightAttachTransform.localRotation = initialRightRotation;
            leftAttachTransform.localRotation = initialLeftRotation;
        }

        if (lastSecondSelector == "RightHand Controller")
        {
            rightHandSecondGunGB.SetActive(false);
            rightHandPlayerGB.SetActive(true);
        }
        else if (lastSecondSelector == "LeftHand Controller")
        {
            leftHandSecondGunGB.SetActive(false);
            leftHandPlayerGB.SetActive(true);
        }
    }

    private void OnSecondHandGrab(XRBaseInteractor interactor)
    {        
        secondInteractor = interactor;
        lastSecondSelector = interactor.gameObject.name;


        if (lastSecondSelector == "RightHand Controller")
        {
            rightHandSecondGunGB.SetActive(true);
            rightHandPlayerGB.SetActive(false);
        }
        else if (lastSecondSelector == "LeftHand Controller")
        {
            leftHandSecondGunGB.SetActive(true);
            leftHandPlayerGB.SetActive(false);
        }
    }

    public override void ProcessInteractable(XRInteractionUpdateOrder.UpdatePhase updatePhase)
    {
        if (selectingInteractor && secondInteractor)
        {
            // Get the Rotation depending on the type selected
            selectingInteractor.attachTransform.rotation = GetTwoHandRotation();

        }
        base.ProcessInteractable(updatePhase);
    }

    /// <summary>
    /// 1 type: basically no rotation apart from just aimming
    /// 2 type: rotation in function of the first controller
    ///  3type: rotation in function of the second controller
    /// </summary>
    /// <returns></returns>
    private Quaternion GetTwoHandRotation()
    {
        Quaternion targetRotation;

        if (twoHandRotationType == TwoHandRotationType.None)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position + offset - selectingInteractor.transform.position);
        }
        else if (twoHandRotationType == TwoHandRotationType.First)
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position + offset - selectingInteractor.transform.position, selectingInteractor.transform.up);
        }
        else
        {
            targetRotation = Quaternion.LookRotation(secondInteractor.attachTransform.position + offset - selectingInteractor.transform.position, secondInteractor.attachTransform.up);

        }
        return targetRotation;
    }

    protected override void OnSelectEntered(XRBaseInteractor interactor)
    {
        base.OnSelectEntered(interactor);
        //Store the initial rotation states;
        initialRightRotation = rightAttachTransform.localRotation;
        initialLeftRotation = leftAttachTransform.localRotation;

        if (selectingInteractor != null)
        {
            lastMainSelector = selectingInteractor.transform.name;
            if (lastMainSelector == "RightHand Controller")
            {
                rightHandMainObjectGB.SetActive(true);
                rightHandPlayerGB.SetActive(false);
            }
            else if (lastMainSelector == "LeftHand Controller")
            {
                leftHandMainObjectGB.SetActive(true);
                leftHandPlayerGB.SetActive(false);
            }

        }
    }

    protected override void OnSelectExited(XRBaseInteractor interactor)
    {
        secondInteractor = null;


        //Change the rotations to retain them
        rightAttachTransform.localRotation = initialRightRotation;
        leftAttachTransform.localRotation = initialLeftRotation;
        base.OnSelectExited(interactor);

        /*
        if (lastMainSelector == "RightHand Controller")
        {
            rightHandMainObjectGB.SetActive(false);
            rightHandPlayerGB.SetActive(true);
        }
        else if (lastMainSelector == "LeftHand Controller")
        {
            leftHandMainObjectGB.SetActive(false);
            leftHandPlayerGB.SetActive(true);
        }*/

        rightHandMainObjectGB.SetActive(false);
        rightHandPlayerGB.SetActive(true);
        leftHandMainObjectGB.SetActive(false);
        leftHandPlayerGB.SetActive(true);

        rightHandSecondGunGB.SetActive(false);      
        leftHandSecondGunGB.SetActive(false);
    }

    /// <summary>
    /// This function will prevent the second grab to stole the object from the first grab in order to have two grabbables
    /// </summary>
    public override bool IsSelectableBy(IXRSelectInteractor interactor)
    {
        
        if (selectingInteractor != null)
        {
            if (selectingInteractor.transform.name == "LeftInventory" || selectingInteractor.transform.name == "RightInventory")
            {
                return base.IsSelectableBy(interactor);

            }

        }
        bool isAlreadyGrabbed = selectingInteractor && !interactor.Equals(selectingInteractor);
        return base.IsSelectableBy(interactor) && ! isAlreadyGrabbed;
    }


}
