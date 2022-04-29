using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomScript : MonoBehaviour
{
    [Header("0 = Up, 1= Down, 2 = Right, 3 = Left")]
    public GameObject[] walls;
    public GameObject[] doors;

    [SerializeField] private Animator[] doorsAnimators;

    [HideInInspector]
    public int posI, posJ;

    private bool enemiesShown = false;
    private bool playerInRoom = false;
    private GameObject enemiesGB;

    private bool mapUpdated = false;
    private bool openSoundedOnce = false;

    private MazeGenerator mazeGeneratorScript;

    private void Start()
    {
        enemiesGB = transform.Find("Enemies").gameObject;
        mazeGeneratorScript = transform.parent.GetComponentInParent<MazeGenerator>();
    }
    private void Update()
    {
        if (playerInRoom)
        {
            
            if (enemiesGB.transform.childCount == 0)
            {
                
                GameManager.openDoors = true;             
                GameManager.HideVelociraptos(); 
            }
            else
            {
                if (!mapUpdated)
                {
                    GameManager.closeDoors = true;
                }
                if (!enemiesShown)
                {                  
                    enemiesShown = true;
                    for (int i = 0; i < enemiesGB.transform.childCount; i++)
                    {
                        if (enemiesGB.transform.GetChild(i).tag == "VelociRaptorMain")
                        {
                            StartCoroutine(enemiesGB.transform.GetChild(i).GetComponent<VelociRaptNav>().Show());
                            break;
                        }                   
                    }
                }
            }

            if (!mapUpdated) // Room not visited previously
            {
                mazeGeneratorScript.UpdateMap(posI, posJ);
                GameManager.ChangeScore("room");
                mapUpdated = true;
                if (transform.tag != "StartRoom" && transform.tag != "ChestRoom")
                {
                    FindObjectOfType<OwnAudioManager>().Play("Doors");
                }

            }
        }
        /*
        Debug.Log(transform.name + " gm " + GameManager.roomCleared);
        
        if (!GameManager.roomCleared)
        {
            CloseDoors();
        }
        else
        {
            OpenDoors();
        }*/
    }
    public void OpenDoors()
    {
        foreach (var door in doorsAnimators)
        {
            door.GetComponent<Animator>().SetBool("OpenDoor", true);
            door.GetComponent<Animator>().SetBool("CloseDoor", false);

        }

        //Needed to play open sound but when going to a visited room we dont want to be played again
        if (transform.tag != "ChestRoom" && !openSoundedOnce && playerInRoom)
        {
            FindObjectOfType<OwnAudioManager>().Play("Doors");
            openSoundedOnce = true;          
        }
        playerInRoom = false;

    }
    public void CloseDoors()
    {
            foreach (var door in doorsAnimators)
            {
                door.GetComponent<Animator>().SetBool("CloseDoor", true);
                door.GetComponent<Animator>().SetBool("OpenDoor", false);

            }                       
    }

    public void PlayerInRoom()
    {
        playerInRoom = true;
    }

    public void UpdateRoom(bool[] status) {
        for (int i = 0; i < status.Length; i++)
        {
            walls[i].SetActive(!status[i]);
            doors[i].SetActive(status[i]);
        }
    }

    public void WallStatus(bool[] status)
    {
        for (int i = 0; i < status.Length; i++)
        {
            walls[i].SetActive(status[i]);
        }
    }
    public void DoorStatus(bool[] status)
    {
        for (int i = 0; i < status.Length; i++)
        {
            doors[i].SetActive(status[i]);
        }
    }

}
