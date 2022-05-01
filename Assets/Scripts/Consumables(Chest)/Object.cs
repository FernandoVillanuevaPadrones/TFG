using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OffsetGrab))]
public class Object : MonoBehaviour
{
    //public ScriptableObjectConsumable objectStats;

    /*
    public enum ObjectType { fireRate, damage, speed, health, cooldown, special}
    public enum SpecialType { Nothing, DoubleShoot, TripleShoot, Infinity, BombShoot, HolyShoot}

    public ObjectType type = ObjectType.fireRate;
    public SpecialType special = SpecialType.Nothing;
    */

    
    public ScriptableObjectConsumable.ObjectType type;
    public ScriptableObjectConsumable.SpecialType special;
    public float _ammountToChange;
    [SerializeField]
    private Renderer objectRenderer;

    public void SetStats(ScriptableObjectConsumable objectStats)
    {
        this._ammountToChange = objectStats._ammountToChange;
        this.type = objectStats.type;
        this.special = objectStats.special;
       // objectRenderer.material.color = objectStats.objectColor;
    }


}
