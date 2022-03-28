using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Materials")]
    [SerializeField] private Material _velociMaterial;
    public static Material velociMaterial;

    private void Start()
    {
        velociMaterial = _velociMaterial;   
    }
    public static void HideVelociraptos()
    {
        velociMaterial.SetFloat("DissolveProgressFloat", 1f);
    }


}
