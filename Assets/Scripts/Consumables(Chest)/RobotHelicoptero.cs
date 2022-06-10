using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class RobotHelicoptero : MonoBehaviour
{
    [SerializeField]
    private float currentRotationSpeed = 15f;
    [SerializeField]
    private AudioSource beepAudio;
    [SerializeField]
    private AudioSource shotAudio;


    [Header ("PROJECTILE")]
    [SerializeField]
    private SphereProjectile projectile;
    [SerializeField]
    private float projectileSpeed;
    [SerializeField]
    private float projectileDmg;
    [SerializeField]
    private float minTimeToShoot = 1.5f;
    [SerializeField]
    private float maxTimeToShoot = 5;
    [SerializeField]
    private Transform projectilePoint;


    private Transform playerPos;
    private GameObject currentRoomGB;

    private Transform rightHandPosition;
    private Transform leftHandPosition;

    public static bool onRightHand;
    public static bool onLeftHand;
    public static bool doInitialAnimation = false;
    public static bool canLookCamera = false;

    private Vector3 finalPosition = new Vector3(1.295f, 1.75f, 0.033f);

    private Transform cameraPos;
    // Start is called before the first frame update
    void Start()
    {
        playerPos = transform.parent;

        rightHandPosition = transform.parent.Find("Camera Offset/RightHand/RightHand Controller");
        leftHandPosition = transform.parent.Find("Camera Offset/LeftHand/LeftHand Controller");
        cameraPos = transform.parent.Find("Camera Offset/Main Camera");


        transform.localPosition = finalPosition;
    }

    // Update is called once per framea
    void Update()
    {
        if (doInitialAnimation)
        {
            StartCoroutine(DoInitialAnimation());
            doInitialAnimation = false;
        }

        if (currentRoomGB != null)
        {
            if (currentRoomGB.transform.childCount > 0)
            {
                transform.RotateAround(playerPos.position, Vector3.up, currentRotationSpeed * Time.deltaTime);
                transform.LookAt(currentRoomGB.transform.GetChild(0).transform.Find("ShootedPoint").position);
            }
            else
            {
                currentRoomGB = null;
                StopAllCoroutines();
            }
        }
        else if(canLookCamera)
        {
            transform.LookAt(cameraPos);

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Room"))
        {
            var enemiesGB = other.transform.parent.Find("Enemies");

            if (enemiesGB.childCount > 0)
            {
                currentRoomGB = enemiesGB.gameObject;
                StartCoroutine(Shoot()); 
            }
        }
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(Random.Range(minTimeToShoot, maxTimeToShoot));

        SphereProjectile smallballgb = Instantiate(projectile, projectilePoint.position, projectilePoint.rotation, projectilePoint);
        smallballgb.enemyParent = this.gameObject;
        smallballgb.isAllied = true;
        smallballgb.projectileDamage = projectileDmg;
        smallballgb.Launch(projectileSpeed);
        
        shotAudio.volume = GameManager.soundEffectLevel;
        shotAudio.Play();

        yield return Shoot();
    }

    IEnumerator DoInitialAnimation()
    {
        Transform initialPosition = null;
        transform.parent = null;
        if (onRightHand)
        {
            initialPosition = rightHandPosition.transform;
        }
        else if(onLeftHand)
        {
            initialPosition = leftHandPosition.transform;
        }

        transform.DOScale(Vector3.zero, 0f);
        transform.position = initialPosition.position;
        Tween myTween = transform.DOScale(Vector3.one, 2f);
        yield return myTween.WaitForCompletion();
        myTween = transform.DOShakeRotation(2f, 15, 10, 10);
        beepAudio.volume = GameManager.soundEffectLevel;
        beepAudio.Play();
        yield return myTween.WaitForCompletion();
        canLookCamera = true;
        transform.parent = playerPos;
        myTween = transform.DOLocalMove(finalPosition, 2f);
        
    }

}
