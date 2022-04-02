using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class MapGenerator : MonoBehaviour
{
    private Vector2 roomOffset = new Vector2(24.22f, 24.22f);

    private string Seed;
    [SerializeField] private int currentSeed = 0;

    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";

    [SerializeField] private int gridDimensionX;
    [SerializeField] private int gridDimensionZ;

    [SerializeField] private int minRooms;
    [SerializeField] private int maxRooms;

    [SerializeField] private GameObject[] room;
    [SerializeField] private GameObject startRoom;
    [SerializeField] private GameObject[] bossRoom;
    //[SerializeField] private GameObject door;
    //[SerializeField] private GameObject node;

    private int[,] Matrix;
    private int leftRooms;

    private int startRoomI = 20;
    private int startRoomJ = 12;

    private GameObject[,] roomsGBMatrix;

    void Start()
    {
        GetSeeds();

        GenerateMatrix();

        Generate();

    }

    private void GetSeeds()
    {
        if (currentSeed == 0)
        {
            Seed = "";

            for (int i = 0; i < 8; i++)
            {
                Seed += glyphs[Random.Range(0, glyphs.Length)];
            }
            currentSeed = Seed.GetHashCode();
        }
        
        

        Random.InitState(currentSeed);
    
    }

    private void GenerateMatrix()
    {
        // 0 nada 1 puerta +1 sala
        Matrix = new int[gridDimensionX * 2 - 1, gridDimensionZ * 2 - 1];
        roomsGBMatrix = new GameObject[gridDimensionX * 2 - 1, gridDimensionZ * 2 - 1];

        //Base room
        Matrix[gridDimensionX - 1, gridDimensionZ - 1] = 2;

        //Number rooms
        leftRooms = Random.Range(minRooms, maxRooms + 1) - 1;

        PopulateMatrix();

    }

    private void PopulateMatrix()
    {
        // pares salas, impares puertas

        for (int i = 2; i < gridDimensionX * 2 - 3; i = i + 2)
        {
            for (int j = 2; j < gridDimensionZ * 2 - 3; j = j + 2)
            {

                if (Matrix[i,j] != 0)
                {
                    int random = Random.Range(0, 108);

                    if (random < 20)
                    {
                        if (Matrix[i + 2, j] == 0)
                        {
                            Matrix[i + 1, j] = 1; // puerta a la anterior
                            Matrix[i + 2, j] = Matrix[i, j] + 1; // valor de posicion + 1 a la anterior
                            leftRooms--;
                        }
                        else
                        {
                            Matrix[i + 1, j] = 1; //puerta por si hay una sala al lado 
                        }
                    }
                    else if (random < 40)
                    {
                        if (Matrix[i - 2, j] == 0)
                        {
                            Matrix[i - 1, j] = 1; // puerta a la anterior
                            Matrix[i - 2, j] = Matrix[i, j] + 1; // valor de posicion + 1 a la anterior
                            leftRooms--;
                        }
                        else
                        {
                            Matrix[i, j + 1] = 1; //puerta por si hay una sala al lado 
                        }
                    }
                    else if (random < 60)
                    {
                        if (Matrix[i, j + 2] == 0)
                        {
                            Matrix[i, j + 1] = 1; // puerta a la anterior
                            Matrix[i, j + 2] = Matrix[i, j] + 1; // valor de posicion + 1 a la anterior
                            leftRooms--;
                        }
                        else
                        {
                            Matrix[i, j + 1] = 1; //puerta por si hay una sala al lado 
                        }
                    }
                    else if (random < 80)
                    {
                        if (Matrix[i, j - 2] == 0)
                        {
                            Matrix[i, j - 1] = 1; // puerta a la anterior
                            Matrix[i, j - 2] = Matrix[i, j] + 1; // valor de posicion + 1 a la anterior
                            leftRooms--;
                        }
                        else
                        {
                            Matrix[i, j - 1] = 1; //puerta por si hay una sala al lado 
                        }
                    }
                    
                    else if (random < 100)
                    {

                    }/*
                    else if (random < 102)
                    {   
                        if (Matrix[i + 2, j] == 0)
                        {
                            Matrix[i + 2, j] = Matrix[i, j]; 
                            Matrix[i + 1, j] = Matrix[i, j];  
                        }
                        else
                        {
                            Matrix[i + 1, j] = 1; 
                        }
                    }
                    else if (random < 104)
                    {   
                        
                        if (Matrix[i - 2, j] == 0)
                        {
                            Matrix[i - 2, j] = Matrix[i, j];
                            Matrix[i - 1, j] = Matrix[i, j];  
                        }
                        else
                        {
                            Matrix[i - 1, j] = 1; 
                        }
                    }
                    else if (random < 106)
                    {
                        if (Matrix[i, j + 2] == 0)
                        {
                            Matrix[i, j + 2] = Matrix[i, j];
                            Matrix[i, j + 1] = Matrix[i, j];  
                        }
                        else
                        {
                            Matrix[i, j + 1] = 1; 
                        }
                    }
                    else
                    {
                        
                        if (Matrix[i, j - 2] == 0)
                        {
                            Matrix[i, j - 2] = Matrix[i, j];
                            Matrix[i, j - 1] = Matrix[i, j];  
                        }
                        else
                        {
                            Matrix[i, j - 1] = 1; 
                        }
                        
                    }*/

                    if (leftRooms <= 0 )
                    {
                        return;
                    }
                }
            }
        }

        // volvemos a generar en caso de que queden salas
        if (leftRooms > 0)
        {
            PopulateMatrix(); 
        }

    }

    private void Generate()
    {

        GameObject rooms = new GameObject();
        rooms.name = "Rooms";
        rooms.transform.position = Vector3.zero;
               
        GameObject doors = new GameObject();
        doors.name = "Doors";
        doors.transform.position = Vector3.zero;

        GameObject nodes = new GameObject();
        nodes.name = "Nodes";
        nodes.transform.position = Vector3.zero;

        //Nodes

        for (int i = 0; i < gridDimensionX * 2 -1; i = i + 2)
        {
            for (int j = 0; j < gridDimensionZ * 2 - 1; j = j + 2)
            {
                //Instantiate(node, new Vector3(i / 2f, 0, j / 2f), Quaternion.identity, nodes.transform);
            }
        }

        Vector3 maxValue = Vector3.zero;
        for (int i = 0; i < gridDimensionX * 2 - 1; i = i + 2)
        {
            for (int j = 0; j < gridDimensionZ * 2 - 1; j = j + 2)
            {
                if (maxValue.z < Matrix[i, j])
                {
                    maxValue = new Vector3(i, j, Matrix[i, j]);
                }
            }
        }


        for (int i = 0; i < gridDimensionX * 2 - 1; i++)
        {
            for (int j = 0; j < gridDimensionZ * 2 - 1; j++)
            {
                if (Matrix[i,j] == 1)
                {
                    //var gameobject = Instantiate(door, new Vector3(i  / 2f, 0, j  / 2f), Quaternion.identity, doors.transform);
                    //gameobject.name = i + " " + j;
                }
                else if (Matrix[i, j] != 0)
                {

                    if (i == startRoomI && j == startRoomJ)
                    {
                        roomsGBMatrix[i, j] = Instantiate(startRoom, new Vector3((i * roomOffset.x) / 2f, 0, (j * roomOffset.y) / 2f), Quaternion.identity, rooms.transform);
                    }
                    else if (maxValue.x == i && maxValue.y == j)
                    {
                        var posRoom = Random.Range(0, room.Length);
                        roomsGBMatrix[i, j] = Instantiate(bossRoom[posRoom], new Vector3((i * roomOffset.x) / 2f, 0, (j * roomOffset.y) / 2f), Quaternion.identity, rooms.transform);
                    }
                    else
                    {
                        var posRoom = Random.Range(0, room.Length);
                        roomsGBMatrix[i, j] = Instantiate(room[posRoom], new Vector3((i * roomOffset.x) / 2f, 0, (j * roomOffset.y) / 2f), Quaternion.identity, rooms.transform);
                    }
                    
                    
                    
                    
                }

                //Debug.Log("Sala: " + i + " " + j + "Valor: " + Matrix[i, j]);
            }
        }



        //Instantiate(bossRoom, new Vector3(maxValue.x / 2f, 0, maxValue.y / 2f), Quaternion.identity, rooms.transform);

        //Instantiate(startRoom, new Vector3((gridDimensionX - 1) / 2f, 0, (gridDimensionZ - 1) / 2f), Quaternion.identity, rooms.transform);

        

        //Centrar

        rooms.transform.position = new Vector3(-gridDimensionX * roomOffset.x / 2, 0, -gridDimensionZ * roomOffset.y / 2);
        doors.transform.position = new Vector3(-gridDimensionX * roomOffset.x / 2, 0, -gridDimensionZ * roomOffset.y / 2);
        nodes.transform.position = new Vector3(-gridDimensionX * roomOffset.x / 2, 0, -gridDimensionZ * roomOffset.y / 2);

        for (int i = 2; i < gridDimensionX * 2 - 3; i = i + 2)
        {
            for (int j = 2; j < gridDimensionZ * 2 - 3; j = j + 2)
            {
                //0 = Up, 1= Down, 2 = Right, 3 = Left
                // true = puerta false = pared

                bool[] doorStatus = new bool[4];
                bool[] wallStatus = new bool[4];

                // Si la de la izq tiene sala pongo puerta sino se pone pared
                if (Matrix[i -2, j] != 0)
                {
                    doorStatus[3] = true;
                    wallStatus[3] = false;

                }
                else
                {
                    doorStatus[3] = false;
                    wallStatus[3] = true;

                }
                // si la de arriba tiene sala pongo puerta sino se pone pared
                if (Matrix[i , j + 2] != 0)
                {
                    doorStatus[0] = true;
                    wallStatus[0] = false;
                }
                else
                {
                    doorStatus[0] = false;
                    wallStatus[0] = true;
                }
                
                // Si la de la derecha tiene sala quito pared sino se pone, pero la puerta se quita siempre ya que la pone la otra sala
                if (Matrix[i + 2, j] != 0)
                {
                    wallStatus[2] = false;
                }
                else
                {
                    wallStatus[2] = true;
                }

                // si la de abajo tiene sala quito pared sino se pone , pero la puerta se quita siempre
                if (Matrix[i, j - 2] != 0)
                {
                    wallStatus[1] = false;
                }
                else
                {
                    wallStatus[1] = true;
                }

                
                doorStatus[1] = false;
                doorStatus[2] = false;
                if (roomsGBMatrix[i, j] != null)
                {
                    roomsGBMatrix[i, j].GetComponent<RoomScript>().DoorStatus(doorStatus);
                    roomsGBMatrix[i, j].GetComponent<RoomScript>().WallStatus(wallStatus);
                }

            }
        }

    }

}
