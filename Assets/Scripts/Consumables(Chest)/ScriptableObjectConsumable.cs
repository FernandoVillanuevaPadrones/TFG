using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ConsumbaleObject", menuName = "ScriptableObjectConsumable/ConsumableScriptableObject")]
public class ScriptableObjectConsumable : ScriptableObject
{
    public enum ObjectType { fireRate, damage, speed, health, cooldown, special }
    public enum SpecialType { Nothing, DoubleShoot, TripleShoot, Infinity, BombShoot, HolyShoot }

    public ObjectType type = ObjectType.fireRate;
    public SpecialType special = SpecialType.Nothing;

    public float _ammountToChange;

    public Color objectColor;
}
