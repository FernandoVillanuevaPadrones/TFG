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
            GetComponent<Animator>().speed = 1;
        }
        else if (currentHealth >= 60)
        {
            GetComponent<Animator>().speed = 1.5f;
        }
        else if (currentHealth >= 40)
        {
            GetComponent<Animator>().speed = 2f;
        }
        else if (currentHealth >= 20)
        {
            GetComponent<Animator>().speed = 3f;
        }
        else
        {
            GetComponent<Animator>().speed = 4f;
        }
    }
}
