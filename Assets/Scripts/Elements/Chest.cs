using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Chest : MonoBehaviour
{
    [Header("CHEST TOP SETTINGS")]
    [SerializeField] private Transform ChestTopTransform;
    [SerializeField] private float finalRotXChestTop = -22.543f;
    [SerializeField] private float chestAnimDuration = 2f;
    [SerializeField] private Ease chestEaseType;
    [SerializeField] private Transform keyPlacedTransform;
    [SerializeField] private float finalRotYChestKey = 0f;
    [SerializeField] private float keyAnimDuration = 2f;
    [SerializeField] private Ease keyEaseType;

    [Header("OBJECTS")]
    [SerializeField]
    private List<GameObject> allCapsules;
    //[SerializeField]
    //private ScriptableObjectConsumable[] scriptableObjectsConsumables;
    [SerializeField]
    private List<ScriptableObjectConsumable> scriptableObjectsList;
    [SerializeField] private float finalPosZObjects = 0.00198f;
    [SerializeField] private float objectDuration = 2f;
    [SerializeField] private Ease objectsEaseType;

    private Animator animator;

    private int posCaps = 0;
    private List<int> availablePos = new List<int> { 0,1,2,3,4,5};
    private List<ScriptableObjectConsumable> availableScripts;
    //private List<ScriptableObjectConsumable> availablePos = new List<int>();

    private void Start()
    {
        animator = GetComponent<Animator>();
        OpenChest();
        PlayerPrefs.SetInt("Level", 3);
        /*
        foreach (var item in allCapsules)
        {
            item.GetComponent<Object>().SetStats(scriptableObjectsList[posCaps]);
            
        }*/

        var remain = 6;
        

        while (remain > 0)
        {
            remain--;
            var pos = Random.Range(0, scriptableObjectsList.Count);     
            allCapsules[posCaps].GetComponent<Object>().SetStats(scriptableObjectsList[pos]); ;
            scriptableObjectsList.RemoveAt(pos);
            posCaps++;
            
            
        }



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
        {
            animator.SetBool("Open", true);
        }       
    }

    public void OpenChest()
    {
        StartCoroutine(OpenChestAnimation());
    }

    private IEnumerator OpenChestAnimation()
    {
        yield return new WaitForSeconds(1);

        Tween chestTween = keyPlacedTransform
            .DOLocalRotate(new Vector3(keyPlacedTransform.localEulerAngles.x, finalRotYChestKey, keyPlacedTransform.localEulerAngles.z), keyAnimDuration)
            .SetEase(keyEaseType);
        yield return chestTween.WaitForCompletion();
         
        chestTween = ChestTopTransform
            .DORotate(new Vector3(finalRotXChestTop, 0f, 0f), chestAnimDuration)
            .SetEase(chestEaseType);
        yield return chestTween.WaitForCompletion();



        if (PlayerPrefs.GetInt("Level") >= 6)
        {
            foreach (var item in allCapsules)
            {
                chestTween = item.transform
                    .DOLocalMoveZ(finalPosZObjects, objectDuration)
                    .SetEase(objectsEaseType);
                yield return chestTween.WaitForCompletion();
                item.GetComponent<OffsetGrab>().enabled = true;
            }
        }
        else if (PlayerPrefs.GetInt("Level") >= 3)
        {
            var remain = 3;

            while (remain > 0)
            {               
                remain--;
                var pos = availablePos[Random.Range(0, availablePos.Count)];
                chestTween = allCapsules[pos].transform
                        .DOLocalMoveZ(finalPosZObjects, objectDuration)
                        .SetEase(objectsEaseType);
                yield return chestTween.WaitForCompletion();
                allCapsules[pos].GetComponent<OffsetGrab>().enabled = true;

                availablePos.Remove(pos);
            }                                
        }        
        else
        {
            var pos = availablePos[Random.Range(0, availablePos.Count)];
            chestTween = allCapsules[pos].transform
                    .DOLocalMoveZ(finalPosZObjects, objectDuration)
                    .SetEase(objectsEaseType);
            yield return chestTween.WaitForCompletion();
            allCapsules[pos].GetComponent<OffsetGrab>().enabled = true;
        }



    }

    public void HideCapsules(GameObject capsulePicked)
    {
        allCapsules.Remove(capsulePicked);
        StartCoroutine(HideCapsulesAnim());
    }
    private IEnumerator HideCapsulesAnim()
    {
        yield return new WaitForSeconds(0.5f);
        Tween chestTween;
        foreach (var item in allCapsules)
        {
            chestTween = item.transform
                .DOLocalMoveZ(3.825664e-05f, objectDuration)
                .SetEase(objectsEaseType);
            item.GetComponent<OffsetGrab>().enabled = false;                   
        }

    }

}
