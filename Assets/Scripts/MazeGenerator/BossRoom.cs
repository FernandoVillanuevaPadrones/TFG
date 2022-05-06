using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    [SerializeField] private Animator capsuleAnimator;
    [SerializeField] private AudioSource capsuleAudio;

    private GameObject enemiesGB;

    private bool sound = false;
    private void Start()
    {
        enemiesGB = transform.Find("Enemies").gameObject;
        
    }
    private void Update()
    {
        if (enemiesGB.transform.childCount == 0 && !sound)
        {
            BossKilled();
            sound = true;
        }
    }

    private void BossKilled()
    {
        capsuleAnimator.SetBool("OpenFloor", true);
        capsuleAudio.Play();
    }
}
