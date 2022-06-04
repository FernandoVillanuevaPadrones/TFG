using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.AI;


public class CamaraVolador : BaseEnemyNav
{
    [Header("Camera Enemy Settings")]
    [SerializeField] private SphereProjectile ballGB;
    [SerializeField] private Transform bulletTransf;
    [SerializeField] private float projectileSpeed = 1f;
    [SerializeField] private float projectileSize = 1.5f;
    [SerializeField] private float maxRangeSpawn = 3f;
    [SerializeField] private float rangeRandomPos = 5.0f;
    [SerializeField] private bool isBoss = false;
    [SerializeField] private GameObject smallCameras;

    private BoxCollider boxCollider;

    private SphereProjectile smallballgb;

    private bool followPlayer = false;
    private bool firstAnim = false;
    private bool shootedSmall = false;
    private bool movingSmall = false;
    private int maxSpawns = 5;

    private AudioSource audioSource;

    public override void Start()
    {
        base.Start();

        if (!isBoss)
        {
            followPlayer = true;
            
        }
        boxCollider = GetComponent<BoxCollider>();
        boxCollider.enabled = false;

        audioSource = GetComponent<AudioSource>();

    }

    private void Update()
    {
        audioSource.volume = GameManager.soundEffectLevel;
        if (playerInRoom || !isBoss)
        {
            boxCollider.enabled = true;
            if (!firstAnim && isBoss)
            {
                firstAnim = true;
                StartCoroutine(FirstAnimation());
            }

            if (followPlayer)
            {
                transform.LookAt(player.position - new Vector3(0f, 0.5f, 0f));

                if (!isBoss)
                {
                    if (!shootedSmall)
                    {
                        StartCoroutine(ShootSmall());
                        shootedSmall = true;
                    }
                    else
                    {
                        float dist = navAgent.remainingDistance;
                        Debug.DrawRay(navAgent.destination, Vector3.up, Color.blue, 1.0f);

                        if (movingSmall)
                        {
                            if (dist != Mathf.Infinity && navAgent.pathStatus == NavMeshPathStatus.PathComplete && GetComponent<NavMeshAgent>().remainingDistance == 0)
                            {
                                shootedSmall = false;
                                movingSmall = false;
                        
                            }
                        }
                    }
                }
            }
        }
    }



    IEnumerator FirstAnimation()
    {
        Tween myTween = transform.DOMoveY(4f, 1.5f);      
        yield return myTween.WaitForCompletion();
        SetNavOffset(4f);
        myTween = transform.DOLookAt(player.position, 2);
        yield return myTween.WaitForCompletion();
        followPlayer = true;
        StartCoroutine(ShootBooss());
    }

    IEnumerator ShootBooss()
    {


        SphereProjectile ballgb = Instantiate(ballGB, bulletTransf.position, bulletTransf.rotation, bulletTransf);
        ballgb.enemyParent = this.gameObject;

        Tween myTween = ballgb.transform.DOScale(new Vector3(1f, 1f, 1f) * projectileSize, 2f);
        yield return myTween.WaitForCompletion();

        
        ballgb.projectileDamage = GetCurrentDamageStat();
        ballgb.Launch(projectileSpeed);

        audioSource.Play();

        yield return new WaitForSeconds(1f);
        StartCoroutine(Spawner());

        yield return new WaitForSeconds(20f);
        StartCoroutine(ShootBooss());
    }
    
    IEnumerator ShootSmall()
    {

        yield return new WaitForSeconds(2f);


        SphereProjectile smallballgb = Instantiate(ballGB, bulletTransf.position, bulletTransf.rotation, bulletTransf);
        smallballgb.enemyParent = this.gameObject;

        Tween myTween = smallballgb.transform.DOScale(new Vector3(1f, 1f, 1f) * projectileSize, 2f);
        yield return myTween.WaitForCompletion();
        
        smallballgb.projectileDamage = GetCurrentDamageStat();
        smallballgb.Launch(projectileSpeed);

        audioSource.Play();

        shootedSmall = true;
        
        navAgent.destination = GetRandomPos(transform.position, rangeRandomPos);
        movingSmall = true;


        yield return new WaitForSeconds(3f);
        StartCoroutine(ShootSmall());
     }

    IEnumerator Spawner()
    {
        var spawned = 0;
        while (spawned < maxSpawns)
        {
            var posX = Random.Range(-maxRangeSpawn, maxRangeSpawn);          
            var posZ = Random.Range(-maxRangeSpawn, maxRangeSpawn);
            var spawnObj = Instantiate(smallCameras, new Vector3(transform.position.x, 0f, transform.position.z )- new Vector3(posX, 0, posZ), Quaternion.identity, transform.parent); //transform.parent == Enemies para que haya que matar a todos los enemigos pa acabar la sala
            yield return new WaitForSeconds(0f); //Needed bc if not there is an error
            var posY = Random.Range(1f, 12f);
            spawnObj.GetComponent<CamaraVolador>().SetNavOffset(posY);
            spawnObj.transform.localScale = new Vector3(0f, 0f, 0f);
            yield return spawnObj.transform.DOScale(0.5f, 2f).WaitForCompletion();            
            spawned++;
            yield return new WaitForSeconds(4f);
        }
    }

    /*
    public void Shoot()
    {
        Instantiate(ballGB, transform.position, transform.localRotation);
    }*/

}
