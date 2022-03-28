using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    [Header("0 = Up, 1= Down, 2 = Right, 3 = Left")]
    public GameObject[] walls;
    public GameObject[] doors;

    [SerializeField] private Animator[] doorsAnimators; 

    private bool enemiesShown = false;
    private bool playerInRoom = false;
    private GameObject enemiesGB;

    private void Start()
    {
        enemiesGB = transform.Find("Enemies").gameObject;
    }
    private void Update()
    {
        if (playerInRoom)
        {
            if (enemiesGB.transform.childCount == 0)
            {
                OpenDoors();
                playerInRoom = false;
                GameManager.HideVelociraptos();
            }
            else
            {
                if (!enemiesShown)
                {
                    enemiesShown = true;
                    for (int i = 0; i < enemiesGB.transform.childCount; i++)
                    {
                        Debug.Log("Hijos: " + enemiesGB.transform.childCount);
                        if (enemiesGB.transform.GetChild(i).tag == "VelociRaptorMain")
                        {
                            StartCoroutine(enemiesGB.transform.GetChild(i).GetComponent<VelociRapt>().Show());
                            break;
                        }                   
                    }
                }
            }
        }
    }
    public void OpenDoors()
    {
        foreach (var door in doorsAnimators)
        {
            door.GetComponent<Animator>().SetBool("OpenDoor", true);
            door.GetComponent<Animator>().SetBool("CloseDoor", false);
        }
    }
    public void CloseDoors()
    {
        foreach (var door in doorsAnimators)
        {
            door.GetComponent<Animator>().SetBool("CloseDoor", true);
            door.GetComponent<Animator>().SetBool("OpenDoor", false);
        }

        playerInRoom = true;
    }

    public void UpdateRoom(bool[] status) {
        for (int i = 0; i < status.Length; i++)
        {
            walls[i].SetActive(!status[i]);
            doors[i].SetActive(status[i]);
        }
    }

}
