using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereProjectile : MonoBehaviour
{
    [SerializeField] private float _lifeTime;
    private Rigidbody _rigidBody;
    [HideInInspector]
    public float projectileDamage;
    [HideInInspector]
    public GameObject enemyParent;


    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    public void Launch(float shootForce)
    {
        if (_rigidBody != null)
        {
            _rigidBody.AddRelativeForce(Vector3.forward * shootForce, ForceMode.Impulse);
            transform.parent = null; // we get rid of the parent so the projectile doesnt follow when launched
            Destroy(gameObject, _lifeTime);
        }

    }



    private void OnTriggerEnter(Collider other)
    {

        if (other.transform.tag == "Player")
        {

            other.transform.GetComponentInParent<Player>().ChangeHealth(projectileDamage);
            Destroy(gameObject);
        }
        else if(other.transform.tag != "Room" && other.transform.tag != "Enemy" && other.transform.tag != "Bullet")
        {
            Debug.Log("destroy con " + other.transform.tag + " " + other.transform.name);
            Destroy(gameObject);
        }
    }

}
