using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class NormalProjectile : Projectile
{
    [SerializeField] private float _lifeTime;
    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }
    public override void Init(Weapon weapon)
    {
        base.Init(weapon);
        Destroy(gameObject, _lifeTime);
    }

    public override void Launch()
    {
        base.Launch();
        _rigidBody.AddRelativeForce(Vector3.forward * _weapon.GetShootingForce(), ForceMode.Impulse);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            other.gameObject.GetComponent<BaseEnemyNav>().DoDamage(_weapon.GetDamage());
            Destroy(gameObject);

        }
        else if(other.transform.tag == "EnemyBody"){
            other.gameObject.GetComponent<DamageRedirector>().RedirectDamage(_weapon.GetDamage());
            Destroy(gameObject);
        }
        else if (other.transform.tag == "VelociRaptor")
        {
            var enemyScript = other.gameObject.GetComponentInParent<VelociRaptNav>();
            enemyScript.DoDamage(_weapon.GetDamage());
            if (enemyScript.currentState == VelociRaptNav.State.Idle)
            {
                enemyScript.GetAlerted();
            }
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Coll con " + collision.transform.name);
        Destroy(gameObject);
    }
}
