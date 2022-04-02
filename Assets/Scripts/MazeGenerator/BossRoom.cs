using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    [SerializeField] private Animator capsuleAnimator;

    private GameObject enemiesGB;
    private void Start()
    {
        enemiesGB = transform.Find("Enemies").gameObject;
        
    }
    private void Update()
    {
        if (enemiesGB.transform.childCount == 0)
        {
            BossKilled();
        }
    }

    private void BossKilled()
    {
        capsuleAnimator.SetBool("OpenFloor", true);
    }
}
