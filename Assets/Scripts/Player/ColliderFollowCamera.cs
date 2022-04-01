using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderFollowCamera : MonoBehaviour
{
    [SerializeField] private Transform Camera;
    [SerializeField] private Transform feet;

    private void FixedUpdate()
    {
        gameObject.transform.position = new Vector3(Camera.position.x, feet.position.y, Camera.position.z);
    }
}
