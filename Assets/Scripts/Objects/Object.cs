using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OffsetGrab))]
public class Object : MonoBehaviour
{
    public enum ObjectType { fireRate, damage, speed, health, cooldown, special}
    public enum SpecialType { Nothing, DoubleShoot, TripleShoot, Infinity, BombShoot, HolyShoot}

    public ObjectType type = ObjectType.fireRate;
    public SpecialType special = SpecialType.Nothing;

    public float _ammountToChange;


}
