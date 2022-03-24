using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallCollider : MonoBehaviour
{
    private void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.2f);

        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Wall")
            {
                Destroy(gameObject);
                return;

            }
        }
    }
}
