using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class RoomScript : MonoBehaviour
{
    [Header("0 = Up, 1= Down, 2 = Right, 3 = Left")]
    public GameObject[] walls;
    public GameObject[] doors;
    public VisualEffect[] smokeEffects;

    [SerializeField] private Animator[] doorsAnimators;

    [HideInInspector]
    public int posI, posJ;

    private bool enemiesAdvised = false;
    private bool playerInRoom = false;
    private GameObject enemiesGB;

    private bool RoomVisited = false;
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
                GameManager.canVelociBehaviour = false;             
            }
            else
            {
                if (!RoomVisited)
                {
                    GameManager.closeDoors = true;
                }
                if (!enemiesAdvised)
                {                  
                    enemiesAdvised = true;
                    for (int i = 0; i < enemiesGB.transform.childCount; i++)
                    {
                        enemiesGB.transform.GetChild(i).GetComponent<BaseEnemyNav>().PlayerInRoom();
                        if (enemiesGB.transform.GetChild(i).tag == "Enemies/VelociRaptorMain")
                        {
                            GameManager.startVelociBehaviour = true;
                        }

                    }
                }
            }

            if (!RoomVisited) // Room not visited previously
            {
                
                GameManager.ChangeScore("room");
                RoomVisited = true;
                if (transform.tag != "StartRoom" && transform.tag != "ChestRoom")
                {
                    FindObjectOfType<OwnAudioManager>().Play("Doors");
                }

            }

            //needed to move always the map
            mazeGeneratorScript.UpdateMap(posI, posJ);
        }

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
            foreach (var effect in smokeEffects)
            {
                effect.Play();
            }
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
