using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereProjectile : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    private Rigidbody _rigidBody;
    [HideInInspector]
    public float projectileDamage;


    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }


    public void Launch(float shootForce)
    {
        _rigidBody.AddRelativeForce(Vector3.forward * shootForce, ForceMode.Impulse);
        Destroy(gameObject, _lifeTime);

    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag == "Player")
        {
            other.transform.GetComponentInParent<Player>().ChangeHealth(projectileDamage);
            Destroy(gameObject);
        }
        else if(other.transform.tag != "Room")
        {
            Destroy(gameObject);
        }
    }

}
