using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateDeactivateRooms : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        

        
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.CompareTag("MapScreen"))
        {
            transform.GetChild(0).gameObject.SetActive(true);
        }
        if (other.transform.CompareTag("MapBorders"))
        {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
