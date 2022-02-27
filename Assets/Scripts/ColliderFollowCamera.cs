using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderFollowCamera : MonoBehaviour
{
    [SerializeField] private GameObject Camera;

    private void FixedUpdate()
    {
        transform.position = Camera.transform.position;
    }
}
