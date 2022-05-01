using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRedirector : MonoBehaviour
{
    [SerializeField] private BaseEnemyNav enemyScript;


    public void RedirectDamage(float damage) {
        enemyScript.DoDamage(damage);
    }
}
