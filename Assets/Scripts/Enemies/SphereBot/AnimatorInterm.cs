using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorInterm : MonoBehaviour
{
    private SphereBot sphereBotScript;


    private void Start()
    {
        sphereBotScript = GetComponentInParent<SphereBot>();
    }
    public void StartShooting()
    {
        sphereBotScript.AttackState(true);
    }
    public void StopShooting()
    {
        sphereBotScript.AttackState(false);
    }

    public void FinishAnim(string boolName)
    {
        sphereBotScript.FinishedAnimation(boolName);
    }

}
