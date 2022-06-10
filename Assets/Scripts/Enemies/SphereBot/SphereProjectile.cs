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
    [HideInInspector]
    public bool isAllied = false;



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
        //Enemy bullet
        if (!isAllied)
        {
            if (other.transform.tag == "Player")
            {
                other.transform.GetComponentInParent<Player>().ChangeHealth(projectileDamage);
                Destroy(gameObject);
            }
            else if(other.transform.tag != "Room" && other.transform.tag != "Enemy" && other.transform.tag != "Bullet")
            {
                Destroy(gameObject);

            }
        }
        //Robot projectile
        else
        {
            Debug.Log("taggg " + other.transform.tag);
            if (other.transform.tag == "Enemy")
            {
                other.gameObject.GetComponent<BaseEnemyNav>().DoDamage(projectileDamage);
                Destroy(gameObject);

            }
            else if (other.transform.tag == "EnemyBody")
            {
                other.gameObject.GetComponent<DamageRedirector>().RedirectDamage(projectileDamage);
                Destroy(gameObject);
            }
            else if (other.transform.tag == "VelociRaptor")
            {
                var enemyScript = other.gameObject.GetComponentInParent<VelociRaptNav>();
                enemyScript.DoDamage(projectileDamage);
                if (enemyScript.currentState == VelociRaptNav.State.Idle)
                {
                    enemyScript.GetAlerted();
                }
                Destroy(gameObject);
            }
        }
    }

}
