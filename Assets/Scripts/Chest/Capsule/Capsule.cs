using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Capsule : MonoBehaviour
{
    private Animator capsuleAnimator;
    private GameManager gameManagerScript;
    

    private void Start()
    {
        capsuleAnimator = transform.GetComponent<Animator>();
        gameManagerScript = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    public void NextAnimState()
    {
        capsuleAnimator.SetBool("NextAnim", true);
    }

    public void StopNextAnimState()
    {
        capsuleAnimator.SetBool("NextAnim", false);
    }

    public void NextLevel()
    {
        gameManagerScript.NextLevel();
    }

    public void asdsa(UnityEvent sd)
    {
        sd.Invoke();
    }

}
