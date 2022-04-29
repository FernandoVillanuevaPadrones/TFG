using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileBase : MonoBehaviour
{
    protected BaseEnemyNav _enemy;

    public virtual void Init(BaseEnemyNav enemy)
    {
        _enemy = enemy;
    }

    public virtual void Launch(float speed)
    {
    }
}
