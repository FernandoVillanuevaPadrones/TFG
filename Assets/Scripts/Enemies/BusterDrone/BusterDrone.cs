using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class BusterDrone : BaseEnemyNav
{
    [Header("Buster Settings")]

    [SerializeField]
    private float minRotationSpeed = 10;
    [SerializeField]
    private float maxRotationSpeed = 20;
    [SerializeField]
    private float minHeight = 1;
    [SerializeField]
    private float maxHeight = 6.5f;
    [SerializeField]
    private float minDistanceToPlayer = 4;
    [SerializeField]
    private float maxDistanceToPlayer = 9;
    [SerializeField]
    private float minTimeToShoot = 1.5f;
    [SerializeField]
    private float maxTimeToShoot = 5;
    [SerializeField]
    private float shootInterval = 1;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private SphereProjectile projectile;
    [SerializeField]
    private Transform projectilePoint;

    [SerializeField]
    private AudioSource shootSource;
    [SerializeField]
    private AudioSource flySource;


    private BoxCollider boxCollider;
    private Rigidbody rigidbody;

    private float rotateDirection = 1f;
    private float currentRotationSpeed = 15f;
    private float currentDistance;

    private Animator animator;
    private bool canShoot = false;
    private bool canMove = false;
    private float currentHeight;

    private bool randomPos = false;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;
        RecalculateDistance();

        rigidbody = GetComponent<Rigidbody>();
    }

    

    // Update is called once per frame
    void Update()
    {
        if (!randomPos)
        {
            //needed to give the enemy a random pos inside the room. Can not be done earlier bc of navagent and room position changing
            randomPos = true;
            transform.position = GetRandomPos(transform.position, 7f) +  new Vector3(0, 0.75f, 0);
            navAgent.enabled = false;
        }
        if (playerInRoom)
        {
            flySource.volume = GameManager.soundEffectLevel;
            if (!flySource.isPlaying)
            {
                flySource.Play();
            }

            rigidbody.isKinematic = false;
            navAgent.enabled = false;
            
            animator.SetBool("Fly", true);
            if (canMove)
            {
                boxCollider.enabled = true;
                transform.LookAt(player.position - new Vector3(0f, 0.5f, 0f));
                transform.RotateAround(GetPlayerPos(), Vector3.up, currentRotationSpeed * Time.deltaTime * rotateDirection);

            
                if (Vector3.Distance(transform.position, GetPlayerPos()) >= (currentDistance + 0.2f))
                {
                    transform.position =  Vector3.MoveTowards(transform.position, GetPlayerPos(), 1f * Time.deltaTime);
                }
                else if (Vector3.Distance(transform.position, GetPlayerPos()) <= (currentDistance - 0.2f ))
                {
                    transform.position = Vector3.MoveTowards(transform.position, GetPlayerPos(), -2f * Time.deltaTime); 
                }

                transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, currentHeight, transform.position.z), 1f * Time.deltaTime);

                if (canShoot)
                { 
                    StartCoroutine(Shoot());
                    canShoot = false;
                }
            }
        }


    }

    //Called in anim
    public void FinishTakeOff()
    {
        StartCoroutine(LookAtPlayer());

    }

    private IEnumerator LookAtPlayer()
    {
        Tween myTween = transform.DOLookAt(player.position, 1);
        yield return myTween.WaitForCompletion();
        canShoot = true;
        canMove = true;
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(Random.Range(minTimeToShoot, maxTimeToShoot));

        for (int i = 0; i < 3; i++)
        {

            SphereProjectile smallballgb = Instantiate(projectile, projectilePoint.position, projectilePoint.rotation, projectilePoint);
            smallballgb.enemyParent = this.gameObject;
            smallballgb.projectileDamage = GetCurrentDamageStat();
            smallballgb.Launch(projectileSpeed);
            shootSource.volume = GameManager.soundEffectLevel;
            shootSource.Play();
            yield return new WaitForSeconds(shootInterval);
        }

        yield return Shoot();
    }

    private void ReCalculateRotationSpeed()
    {
        currentRotationSpeed = Random.Range(minRotationSpeed, maxRotationSpeed);

    }

    private void RecalculateHeight()
    {
        currentHeight = Random.Range(minHeight, maxHeight);
    }

    private void RecalculateDistance()
    {
        currentDistance = Random.Range(minDistanceToPlayer, maxDistanceToPlayer);

    }

    private void OnCollisionEnter(Collision collision)
    {

        rotateDirection *= -1;
        ReCalculateRotationSpeed();
        RecalculateDistance();
        RecalculateHeight();


    }

    private void OnTriggerEnter(Collider other)
    {
        rotateDirection *= -1;
        ReCalculateRotationSpeed();
        RecalculateDistance();
        RecalculateHeight();

    }
}
