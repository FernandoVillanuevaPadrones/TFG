using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Capsule : MonoBehaviour
{
    private Animator capsuleAnimator;

    private void Start()
    {
        capsuleAnimator = transform.GetComponent<Animator>();
    }
    public void NextAnimState()
    {
        capsuleAnimator.SetBool("NextAnim", true);
    }

    public void StopNextAnimState()
    {
        capsuleAnimator.SetBool("NextAnim", false);
    }

}
