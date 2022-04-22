using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeLine : MonoBehaviour
{

    private Animator animator;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeLineSpeed(float currentHealth)
    {
        if (currentHealth >= 80)
        {
            animator.speed = 1;
        }
        else if (currentHealth >= 60)
        {
            animator.speed = 1.5f;
        }
        else if (currentHealth >= 40)
        {
            animator.speed = 2f;
        }
        else if (currentHealth >= 20)
        {
            animator.speed = 3f;
        }
        else
        {
            animator.speed = 4f;
        }
    }
}
