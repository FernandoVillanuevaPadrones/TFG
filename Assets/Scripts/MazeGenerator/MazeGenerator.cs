using System.Collections.Generic;
using UnityEngine;



public class MazeGenerator : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField]
    private GameObject playerGB;
    [SerializeField]
    private GameObject gunGB;

    [Header("MAZE SETTINGS")]
    [SerializeField] private Vector2 roomOffset = new Vector2(24.22f, 24.22f);    
    [SerializeField] private int currentSeed = 0;
    [SerializeField] private int gridDimensionX;
    [SerializeField] private int gridDimensionZ;


    [SerializeField] private GameObject[] room;
    [SerializeField] private GameObject startRoom;
    [SerializeField] private GameObject chestRoom;
    [SerializeField] private GameObject[] bossRoom;

    [Header("ENEMIES")]
    [SerializeField] private GameObject[] normalEnemies;
    [SerializeField] private GameObject[] bossEnemies;



    [Header("MAP SETTINGS")]
    public bool hideRooms = true;
    [SerializeField] private Vector2 mapOffset = new Vector2(0.05f, 0.05f);
    [SerializeField] private Vector2 doorOffset = new Vector2(0.05f, 0.05f);
    [SerializeField] private GameObject mapObject;
     private GameObject mapHUDObject;
    [SerializeField] private float mapScale = 0.7f;
    [SerializeField] private GameObject roomMap;
    [SerializeField] private GameObject startRoomMap;
    [SerializeField] private GameObject chestRoomMap;
    [SerializeField] private GameObject bossRoomMap;
    [SerializeField] private GameObject doorMap;
    [SerializeField] private GameObject nodeMap;

    [Header("PLAYER IN MAP SETTINGS")]
    [SerializeField] private GameObject playerMapGB;
    [SerializeField] private float playerOffset = 0.001f;

    private int minRooms;
    private int maxRooms;

    private int[,] Matrix;
    private int leftRooms;

    private string Seed;
    const string glyphs = "abcdefghijklmnopqrstuvwxyz0123456789";

    private GameObject[,] roomsGBMatrix;
    private GameObject[,] mapGBMatrix;

    private int startRoomPosI, startRoomPosJ, bossRoomPosI, bossRoomPosJ, chestRoomPosI, chestRoomPosJ;

    private GameObject playerMapInstantiated;

    
    private class availableRooms
    {
        public int posI;
        public int posJ;
    }

    private List<availableRooms> availableRoomsList = new List<availableRooms>();
    private List<GameObject> allRoomsList = new List<GameObject>();


    private int currentLevel;

    void Start()
    {
        
        currentLevel = PlayerPrefs.GetInt("Level");

        minRooms = currentLevel + 2;
        maxRooms = currentLevel + 4;

        mapHUDObject = mapObject.transform.Find("HUDMoveOffset/HUDRotateOffset/HUD").gameObject;

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
        mapGBMatrix = new GameObject[gridDimensionX * 2 - 1, gridDimensionZ * 2 - 1];

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
                    
                    int random = Random.Range(0, 80);

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
                    else
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
        //---------MAZE-----------
        GameObject rooms = new GameObject();
        rooms.name = "Rooms";
        rooms.transform.position = Vector3.zero;
        rooms.transform.parent = transform;


        //---------MAP-----------

        mapObject.transform.localScale = new Vector3(1f, 1f, 1f);


        GameObject roomsMap = new GameObject();
        roomsMap.name = "RoomsMap";
        roomsMap.transform.position = Vector3.zero;
        roomsMap.transform.parent = mapHUDObject.transform;

        GameObject doorsMap = new GameObject();
        doorsMap.name = "DoorsMap";
        doorsMap.transform.position = Vector3.zero;
        doorsMap.transform.parent = mapHUDObject.transform;

        GameObject nodesMap = new GameObject();
        nodesMap.name = "NodesMap";
        nodesMap.transform.position = Vector3.zero;
        nodesMap.transform.parent = mapHUDObject.transform;
        
        GameObject playerMap = new GameObject();
        playerMap.name = "playerMap";
        playerMap.transform.position = Vector3.zero;
        playerMap.transform.parent = mapHUDObject.transform;



        Vector3 maxValue = Vector3.zero;
        Vector3 minValue = new Vector3(0, 0, 100);    


        for (int i = 0; i < gridDimensionX * 2 - 1; i = i + 2)
        {
            for (int j = 0; j < gridDimensionZ * 2 - 1; j = j + 2)
            {
                if (Matrix[i, j] != 0)
                {
                    if (maxValue.z < Matrix[i, j])
                    {
                        maxValue = new Vector3(i, j, Matrix[i, j]);
                    }
                    if (minValue.z > Matrix[i,j])
                    {
                        minValue = new Vector3(i, j, Matrix[i, j]);
                        
                    }
                    
                }
                
            }
        }
    
        for (int i = 0; i < gridDimensionX * 2 - 1; i++)
        {
            for (int j = 0; j < gridDimensionZ * 2 - 1; j++)
            {
                if (Matrix[i,j] == 1)
                {

                }
                else if (Matrix[i, j] != 0)
                {

                    if (minValue.x == i && minValue.y == j)
                    {
                        //------MAZE--------
                        roomsGBMatrix[i, j] = Instantiate(startRoom, new Vector3((i * roomOffset.x) / 2f, 0, (j * roomOffset.y) / 2f), Quaternion.identity, rooms.transform);
                        roomsGBMatrix[i, j].name = "Start: "+ i + " " + j;
                        var roomScript = roomsGBMatrix[i, j].GetComponent<RoomScript>();
                        roomScript.posI = i;
                        roomScript.posJ = j;


                        //------MAP--------
                        mapGBMatrix[i, j] = Instantiate(startRoomMap, new Vector3((i * mapOffset.x) / 2f, (j * mapOffset.y) / 2f, 0), Quaternion.identity, roomsMap.transform);
                        mapGBMatrix[i, j].name = "Start: " + i + " " + j;

                        playerMapInstantiated = Instantiate(playerMapGB, new Vector3((i * mapOffset.x) / 2f, (j * mapOffset.y) / 2f, 0), Quaternion.identity, playerMap.transform);

                        startRoomPosI = i;
                        startRoomPosJ = j;

                    }
                    else if (maxValue.x == i && maxValue.y == j)
                    {
                        //------MAZE--------
                        var posRoom = Random.Range(0, bossRoom.Length);                      
                        roomsGBMatrix[i, j] = Instantiate(bossRoom[posRoom], new Vector3((i * roomOffset.x) / 2f, 0, (j * roomOffset.y) / 2f), Quaternion.identity, rooms.transform);
                        roomsGBMatrix[i, j].name = "Boss: " + i + " " + j;
                        var roomScript = roomsGBMatrix[i, j].GetComponent<RoomScript>();
                        roomScript.posI = i;
                        roomScript.posJ = j;

                        bossRoomPosI = i;
                        bossRoomPosJ = j;

                        Instantiate(bossEnemies[Random.Range(0, bossEnemies.Length)], roomsGBMatrix[i, j].transform.Find("Enemies"));


                        //------MAP--------
                        mapGBMatrix[i, j] = Instantiate(bossRoomMap, new Vector3((i * mapOffset.x) / 2f, (j * mapOffset.y) / 2f, 0), Quaternion.identity, roomsMap.transform);
                        mapGBMatrix[i, j].name = "Boss: " + i + " " + j;

                        if (hideRooms)
                        {
                            mapGBMatrix[i, j].SetActive(false);

                        }
                    }
                    else
                    {
                        //------MAZE--------
                        var posRoom = Random.Range(0, room.Length);
                        roomsGBMatrix[i, j] = Instantiate(room[posRoom], new Vector3((i * roomOffset.x) / 2f, 0, (j * roomOffset.y) / 2f), Quaternion.identity, rooms.transform);
                        roomsGBMatrix[i, j].name = "Normal: " + i + " " + j;
                        var roomScript = roomsGBMatrix[i, j].GetComponent<RoomScript>();
                        roomScript.posI = i;
                        roomScript.posJ = j;

                        

                        var availableRoom = new availableRooms();
                        availableRoom.posI = i;
                        availableRoom.posJ = j;
                        availableRoomsList.Add(availableRoom);


                        //if level less than 3 only one type of enemy is instantiated

                        // if less than 6 or equal, a total of 4 enemies of two types

                        // if not a total of 6 of 3 different enemies

                        Debug.Log("Curr " + currentLevel);

                        if (currentLevel <= 3)
                        {
                            var randomEnemy = Random.Range(0, normalEnemies.Length);
                            for (int u = 0; u < normalEnemies[randomEnemy].GetComponent<BaseEnemyNav>().numberOfEnemiesSameRoom; u++)
                            {
                                GameObject enemy = Instantiate(normalEnemies[randomEnemy], roomsGBMatrix[i, j].transform.Find("Enemies"));
                                enemy.transform.name = enemy.transform.name + " " + u;
                            }
                        }
                        else if (currentLevel <= 6)
                        {
                            var randomEnemy = Random.Range(0, normalEnemies.Length);
                            var lastRandomEnemy = randomEnemy;
                            for (int u = 0; u < 2; u++)
                            {
                                GameObject enemy = Instantiate(normalEnemies[randomEnemy], roomsGBMatrix[i, j].transform.Find("Enemies"));
                                enemy.transform.name = enemy.transform.name + " " + u;
                            }

                            randomEnemy = Random.Range(0, normalEnemies.Length);
                            while (randomEnemy == lastRandomEnemy)
                            {
                                randomEnemy = Random.Range(0, normalEnemies.Length);
                            }

                            for (int u = 0; u < 2; u++)
                            {
                                GameObject enemy = Instantiate(normalEnemies[randomEnemy], roomsGBMatrix[i, j].transform.Find("Enemies"));
                                enemy.transform.name = enemy.transform.name + " " + u;
                            }
                        }
                        else
                        {
                            var randomEnemy = Random.Range(0, normalEnemies.Length);

                            var firstRandomEnemy = randomEnemy;
                            
                            for (int u = 0; u < 2; u++)
                            {
                                GameObject enemy = Instantiate(normalEnemies[randomEnemy], roomsGBMatrix[i, j].transform.Find("Enemies"));
                                enemy.transform.name = enemy.transform.name + " " + u;
                            }

                            randomEnemy = Random.Range(0, normalEnemies.Length);
                            while (randomEnemy == firstRandomEnemy)
                            {
                                randomEnemy = Random.Range(0, normalEnemies.Length);
                            }

                            var secondEnemy = randomEnemy;
                            for (int u = 0; u < 2; u++)
                            {
                                GameObject enemy = Instantiate(normalEnemies[randomEnemy], roomsGBMatrix[i, j].transform.Find("Enemies"));
                                enemy.transform.name = enemy.transform.name + " " + u;
                            }

                            randomEnemy = Random.Range(0, normalEnemies.Length);
                            while (randomEnemy == firstRandomEnemy || randomEnemy == secondEnemy)
                            {
                                randomEnemy = Random.Range(0, normalEnemies.Length);
                            }

                            for (int u = 0; u < 2; u++)
                            {
                                GameObject enemy = Instantiate(normalEnemies[randomEnemy], roomsGBMatrix[i, j].transform.Find("Enemies"));
                                enemy.transform.name = enemy.transform.name + " " + u;
                            }

                            Debug.Log("1 " + firstRandomEnemy);
                            Debug.Log("2 " + secondEnemy);
                            Debug.Log("3 " + randomEnemy);

                        }


                        //------MAP--------
                        mapGBMatrix[i, j] = Instantiate(roomMap, new Vector3((i * mapOffset.x) / 2f, (j * mapOffset.y) / 2f, 0), Quaternion.identity, roomsMap.transform);
                        mapGBMatrix[i, j].name = "Normal: " + i + " " + j;

                        if (hideRooms)
                        {
                            mapGBMatrix[i, j].SetActive(false);

                        }

                    }                                                                             
                    
                    allRoomsList.Add(roomsGBMatrix[i, j]);
                }                
            }
        }
        
        //I am using a custom class because is needed to get the exact i and j pos of the same room, if not, it can appear in an undesired room like the start one
        var randomRoom = Random.Range(0, availableRoomsList.Count);
        chestRoomPosI = availableRoomsList[randomRoom].posI;
        chestRoomPosJ = availableRoomsList[randomRoom].posJ;

        //The selected room is destroyed
        // I do this because chest room is needed100% so I wait to create all the rooms so it is not missed in random probabilities
        var roomObject = roomsGBMatrix[chestRoomPosI, chestRoomPosJ];
        Destroy(roomObject);

        allRoomsList.Remove(roomsGBMatrix[chestRoomPosI, chestRoomPosJ]);

        roomsGBMatrix[chestRoomPosI, chestRoomPosJ] = Instantiate(chestRoom, new Vector3((chestRoomPosI * roomOffset.x) / 2f, 0, (chestRoomPosJ * roomOffset.y) / 2f), Quaternion.identity, rooms.transform);
        roomsGBMatrix[chestRoomPosI, chestRoomPosJ].name = "Chest: " + chestRoomPosI + " " + chestRoomPosJ;
        var chestRoomScript = roomsGBMatrix[chestRoomPosI, chestRoomPosJ].GetComponent<RoomScript>();
        chestRoomScript.posI = chestRoomPosI;
        chestRoomScript.posJ = chestRoomPosJ;

        allRoomsList.Add(roomsGBMatrix[chestRoomPosI, chestRoomPosJ]);

        var roomMapObject = mapGBMatrix[chestRoomPosI, chestRoomPosJ];
        Destroy(roomMapObject);
        mapGBMatrix[chestRoomPosI, chestRoomPosJ] = Instantiate(chestRoomMap, new Vector3((chestRoomPosI * mapOffset.x) / 2f, (chestRoomPosJ * mapOffset.y) / 2f, 0), Quaternion.identity, roomsMap.transform);
        mapGBMatrix[chestRoomPosI, chestRoomPosJ].name = "Chest: " + chestRoomPosI + " " + chestRoomPosJ;

        if (hideRooms)
        {
            mapGBMatrix[chestRoomPosI, chestRoomPosJ].SetActive(false);

        }


        for (int i = 0; i < gridDimensionX * 2 - 1; i = i + 2)
        {
            for (int j = 0; j < gridDimensionZ * 2 - 1; j = j + 2)
            {

                //0 = Up, 1= Down, 2 = Right, 3 = Left
                // true = puerta false = pared

                bool[] doorStatus = new bool[4];
                bool[] wallStatus = new bool[4];

                // Si la de la izq tiene sala pongo puerta sino se pone pared
                if ( i != 0 && Matrix[i -2, j] != 0)
                {


                    doorStatus[3] = true;
                    wallStatus[3] = false;

                    if(Matrix[i, j] != 0)
                    {
                        mapGBMatrix[i - 1, j] = Instantiate(doorMap, new Vector3(((i - 1) * doorOffset.x) / 2f, (j * doorOffset.y) / 2f, 0), Quaternion.identity, doorsMap.transform);
                        mapGBMatrix[i - 1, j].name = i - 1 + " " + j;

                        if (hideRooms)
                        {
                            mapGBMatrix[i - 1, j].SetActive(false);

                        }
                    }
                    
                }
                else
                {
                    doorStatus[3] = false;
                    wallStatus[3] = true;

                }
                // si la de arriba tiene sala pongo puerta sino se pone pared
                // Y si no estamos en la ultima sala posible
                if ((j != gridDimensionZ * 2 - 2) && (Matrix[i , j + 2] != 0))
                {
                    doorStatus[0] = true;
                    wallStatus[0] = false;

                    if (Matrix[i, j] != 0)
                    {
                        mapGBMatrix[i, j + 1] = Instantiate(doorMap, new Vector3((i * doorOffset.x) / 2f, ((j + 1) * doorOffset.y) / 2f, 0), Quaternion.identity, doorsMap.transform);
                        mapGBMatrix[i, j + 1].name = i + " " + (j + 1);
                        mapGBMatrix[i, j + 1].transform.Rotate(0f, 0f, 90f);

                        if (hideRooms)
                        {
                            mapGBMatrix[i, j + 1].SetActive(false);

                        }

                    }
                }
                else
                {
                    doorStatus[0] = false;
                    wallStatus[0] = true;
                }
                
                // Si la de la derecha tiene sala quito pared sino se pone, pero la puerta se quita siempre ya que la pone la otra sala
                if ((i != gridDimensionX * 2 - 2) && Matrix[i + 2, j] != 0)
                {
                    wallStatus[2] = false;

                }
                else
                {
                    wallStatus[2] = true;
                }

                // si la de abajo tiene sala quito pared sino se pone , pero la puerta se quita siempre
                if ( j != 0 && Matrix[i, j - 2] != 0)
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


        //Centrar

        //-------MAZE---------
        rooms.transform.position = new Vector3(-gridDimensionX * roomOffset.x / 2, 0, -gridDimensionZ * roomOffset.y / 2);

        var startRoomPos = roomsGBMatrix[startRoomPosI, startRoomPosJ].transform.position;
        playerGB.transform.position = new Vector3(startRoomPos.x + 2, 1, startRoomPos.z);
        mapObject.transform.position = new Vector3(startRoomPos.x + 2, 1, startRoomPos.z + 1f);
        gunGB.transform.position = new Vector3(startRoomPos.x, 1, startRoomPos.z);

        GameManager.roomsPlaced = true;


        //-------MAP----------
        
        roomsMap.transform.localPosition = new Vector3(-gridDimensionX * mapOffset.x / 2, 0, -gridDimensionZ * mapOffset.y / 2);
        doorsMap.transform.localPosition = new Vector3(-gridDimensionX * doorOffset.x / 2, 0, -gridDimensionZ * doorOffset.y / 2);
        nodesMap.transform.localPosition = new Vector3(-gridDimensionX * mapOffset.x / 2, 0, -gridDimensionZ * mapOffset.y / 2);
        playerMap.transform.localPosition = new Vector3(-gridDimensionX * mapOffset.x / 2, 0, (-gridDimensionZ * mapOffset.y / 2) + playerOffset);

        mapObject.transform.localScale = new Vector3(mapScale, mapScale, mapScale);

        playerMapInstantiated.transform.GetChild(0).GetComponent<PlayerMapMovement>().GetRoomsToMove(roomsMap, doorsMap);

        playerMap.transform.parent = mapObject.transform;
    }

    public void UpdateMap(int i, int j)
    {
        if ( i!= 0 && mapGBMatrix[i - 2, j] != null)
        {
            mapGBMatrix[i - 2, j].SetActive(true);
            mapGBMatrix[i - 1, j].SetActive(true);
        }
        if ((j != gridDimensionZ * 2 - 2) && mapGBMatrix[i, j + 2] != null)
        {
            mapGBMatrix[i, j + 2].SetActive(true);
            mapGBMatrix[i, j + 1].SetActive(true);
        }
        if ((i != gridDimensionX * 2 - 2) && mapGBMatrix[i + 2, j] != null)
        {
            mapGBMatrix[i + 2, j].SetActive(true);
            mapGBMatrix[i + 1, j].SetActive(true);
        }
        if (j != 0 && mapGBMatrix[i, j - 2] != null)
        {
            mapGBMatrix[i, j - 2].SetActive(true);
            mapGBMatrix[i, j - 1].SetActive(true);
        }

        mapHUDObject.GetComponent<ZoomMap>().MoveMapTo(i,j);




    }

    public void ShowAllMap()
    {
        for (int i = 0; i < gridDimensionX  * 2 - 1; i++)
        {
            for (int j = 0; j < gridDimensionZ * 2 - 1; j++)
            {
                if (mapGBMatrix[i, j] != null)
                {
                    mapGBMatrix[i, j].SetActive(true);
                }
            }
        }
    }

    public void OpenAllDoors()
    {
        foreach (GameObject room in allRoomsList)
        {
            room.GetComponent<RoomScript>().OpenDoors();
        }
    }

    public void CloseAllDoors()
    {
        foreach (GameObject room in allRoomsList)
        {
            room.GetComponent<RoomScript>().CloseDoors();
        }
    }

}
