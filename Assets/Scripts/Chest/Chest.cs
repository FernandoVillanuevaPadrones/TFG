using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
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
    [SerializeField] private VisualEffect smokeEffect;

    [Header("OBJECTS")]
    [SerializeField]
    private List<GameObject> allCapsules;

    [Header("Sounds")]
    [SerializeField]
    private AudioClip OpenClip;
    [SerializeField]
    private AudioClip hideCapsulesClip;

    [SerializeField]
    private List<ScriptableObjectConsumable> scriptableObjectsList;
    [SerializeField] private float finalPosZObjects = 0.00198f;
    [SerializeField] private float objectDuration = 2f;
    [SerializeField] private Ease objectsEaseType;

    private Animator animator;

    private int posCaps = 0;
    private List<int> availablePos = new List<int> { 0,1,2,3,4,5};

    private AudioSource audioSource;

    private int hasMapUpgrade = 0;
    private int hasHeli = 0;
    private int currentLevel;
    private void Start()
    {
        currentLevel = PlayerPrefs.GetInt("Level");
        currentLevel = 6;

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        hasMapUpgrade = PlayerPrefs.GetInt("ShowMapUpgrade");
        hasHeli = PlayerPrefs.GetInt("HasHeli");

        var remain = 6;

        while (remain > 0)
        {
            var pos = Random.Range(0, scriptableObjectsList.Count);

            if (hasHeli == 1 && scriptableObjectsList[pos].name == "Heli")
            {
                continue;
            }
            else if(hasMapUpgrade == 1 && scriptableObjectsList[pos].name == "ShowMap")
            {
                continue;
            }

            allCapsules[posCaps].GetComponent<Object>().SetStats(scriptableObjectsList[pos]); ;
            scriptableObjectsList.RemoveAt(pos);
            posCaps++;
            remain--;
            
            
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

        audioSource.clip = OpenClip;
        audioSource.volume = GameManager.soundEffectLevel;
        audioSource.Play();

        chestTween = ChestTopTransform
            .DORotate(new Vector3(finalRotXChestTop, 0f, 0f), chestAnimDuration)
            .SetEase(chestEaseType);

        smokeEffect.Play();
        yield return chestTween.WaitForCompletion();



        if (currentLevel >= 6)
        {
            foreach (var item in allCapsules)
            {
                chestTween = item.transform
                    .DOLocalMoveZ(finalPosZObjects, objectDuration)
                    .SetEase(objectsEaseType);

                item.GetComponent<AudioSource>().volume = GameManager.soundEffectLevel;

                item.GetComponent<AudioSource>().Play();
                yield return chestTween.WaitForCompletion();
                item.GetComponent<OffsetGrab>().enabled = true;
            }
        }
        else if (currentLevel >= 3)
        {
            var remain = 3;

            while (remain > 0)
            {               
                remain--;
                var pos = availablePos[Random.Range(0, availablePos.Count)];
                chestTween = allCapsules[pos].transform
                        .DOLocalMoveZ(finalPosZObjects, objectDuration)
                        .SetEase(objectsEaseType);

                allCapsules[pos].GetComponent<AudioSource>().volume = GameManager.soundEffectLevel;
                allCapsules[pos].GetComponent<AudioSource>().Play();

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

            allCapsules[pos].GetComponent<AudioSource>().volume = GameManager.soundEffectLevel;
            allCapsules[pos].GetComponent<AudioSource>().Play();
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

        //Oonly when there are more objects to pick
        if (currentLevel >= 3)
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
            audioSource.clip = hideCapsulesClip;
            audioSource.volume = GameManager.soundEffectLevel;
            audioSource.Play();

        }

    }

}
