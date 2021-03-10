using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class MapGenerator : MonoBehaviour
{
    public GameObject player;

    [Header("Game Settings")]
    public Vector3 firstTilePosition;
    [Space]
    public GameObject enemy;
    public GameObject tile;
    public GameObject chest;
    public GameObject torch;
    public List<AvailablePositions> allTiles = new List<AvailablePositions>();
    private List<NavMeshSurface> nms = new List<NavMeshSurface>();
    private List<Enemy> allEnemies = new List<Enemy>();

    [Header("UI Info")]
    public float timerTime = 0;
    public GameObject loadingScreen;
    public Slider progress;
    public Text info;
    public Text timer;

    [Header("Map generation settings")]
    public int seed;
    public int mapLength;
    [Range(0, 100)] public int goUpChance;
    [Range(0, 100)] public int goDownChance;
    [Range(0, 100)] public int torchChance;
    public int chests;
    public int enemies;
    [Header("Item settings")]
    public List<ItemDataBase> items = new List<ItemDataBase>();

    void Start()
    {
        UnityEngine.Random.InitState(seed);
        progress.value = 0;
        if ((chests + enemies) > mapLength )
        {
            chests = 0;
            enemies = 0;
            Debug.LogError("Too many chests and/or enemies for the map size, placing no chests/enemies");
        }
        progress.maxValue = mapLength * 6 + chests + enemies + 10;
        info.text = "Creating Dungeon";
        StartCoroutine(Generator());
    }

    GameObject lastTile;
    AvailablePositions ap;
    AvailablePositions lastAp;

    Transform firstTile;
    IEnumerator Generator()
    {
        lastTile = Instantiate(tile, firstTilePosition, Quaternion.identity);
        lastTile.name = "Tile -1";
        ap = lastTile.GetComponent<AvailablePositions>();
        firstTile = lastTile.transform;
        allTiles.Add(ap);
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < mapLength; i++)
        {
            lastAp = ap;
            GameObject t = Instantiate(tile, RandomPosition(), Quaternion.identity);
            t.gameObject.name = "Tile " + i;
            
            ap = t.GetComponent<AvailablePositions>();
            nms.Add(t.transform.GetChild(0).GetComponent<NavMeshSurface>());

            lastTile = t;

            lastTile.transform.SetParent(firstTile);
            
            SetPosition();
            allTiles.Add(ap);
            progress.value++;
            yield return new WaitForEndOfFrame();
        }
        yield return null;
        StartCoroutine(FinalizingMaze());
    }

    IEnumerator FinalizingMaze()
    {
        int r = 0;
        AvailablePositions tempTile = null;
        info.text = "Creating Rooms";
        yield return null;
        foreach (AvailablePositions a in allTiles)
        {
            Vector3 currentPos = Vector3.zero;
            currentPos = a.myPosition;
            currentPos += Vector3.up;
            foreach (AvailablePositions aa in allTiles)
            {
                if (currentPos == aa.myPosition)
                {
                    a.forwardPos.gameObject.SetActive(false);
                    a.hasWallF = false;
                    aa.backwardsPos.gameObject.SetActive(false);
                    aa.hasWallB = false;
                }
            }
            currentPos = a.myPosition;
            currentPos += Vector3.left;
            foreach (AvailablePositions aa in allTiles)
            {
                if (currentPos == aa.myPosition)
                {
                    a.leftPos.gameObject.SetActive(false);
                    a.hasWallL = false;
                    aa.rightPos.gameObject.SetActive(false);
                    aa.hasWallR = false;
                }
            }
            yield return new WaitForEndOfFrame();
            progress.value++;
        }
        info.text = "Spawning Torches";
        yield return new WaitForEndOfFrame();
        foreach (AvailablePositions a in allTiles)
        {
            r = Random.Range(0, 100);
            if (r <= torchChance)
            {
                if (a.hasWallF)
                {
                    a.forwardPos.GetChild(0).gameObject.SetActive(true);
                }
                else if (a.hasWallB)
                {
                    a.backwardsPos.GetChild(0).gameObject.SetActive(true);
                }
                else if (a.hasWallL)
                {
                    a.leftPos.GetChild(0).gameObject.SetActive(true);
                }
                else if (a.hasWallR)
                {
                    a.rightPos.GetChild(0).gameObject.SetActive(true);
                }
            }
            yield return new WaitForEndOfFrame();
            progress.value++;
        }
        info.text = "Spawning Hazards";
        yield return new WaitForEndOfFrame();
        foreach (AvailablePositions a in allTiles)
        {
            if (a.hasHazard)
            {
                a.hasObject = true;
                a.hazard[Random.Range(0, a.hazard.Length)].SetActive(true);
                yield return new WaitForEndOfFrame();
            }
            progress.value++;
        }
        info.text = "Building Navmesh";
        yield return new WaitForEndOfFrame();
        foreach (var i in nms)
        {
            i.BuildNavMesh();
            yield return new WaitForEndOfFrame();
            progress.value++;
        }
        info.text = "Spawning Chests";
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < chests; i++)
        {
            r = Random.Range(0, allTiles.Count);
            tempTile = allTiles[r];
            while (tempTile.hasObject)
            {
                r = Random.Range(0, allTiles.Count);
                tempTile = allTiles[r];
                yield return new WaitForEndOfFrame();
            }
            Transform spawnLocation = tempTile.spawnLocations[Random.Range(0, tempTile.spawnLocations.Length)];
            GameObject T = Instantiate(chest, spawnLocation.position, spawnLocation.rotation).gameObject;
            T.GetComponent<Chest>().itemObject.AddComponent<Item>().Setup(items[Random.Range(0, items.Count)]);
            tempTile.hasObject = true;
            progress.value++;
        }
        info.text = "Placing Enemies";
        yield return new WaitForSeconds(2);
        for (int i = 0; i < enemies; i++)
        {
            r = Random.Range(0, allTiles.Count);
            tempTile = allTiles[r];
            while (tempTile.hasObject)
            {
                r = Random.Range(0, allTiles.Count);
                tempTile = allTiles[r];
                yield return new WaitForEndOfFrame();
            }
            Transform spawnLocation = tempTile.spawnLocations[Random.Range(0, tempTile.spawnLocations.Length)];
            Enemy tempEnemy = Instantiate(enemy, spawnLocation.position + Vector3.up, spawnLocation.rotation).GetComponent<Enemy>();
            allEnemies.Add(tempEnemy);
            tempTile.hasObject = true;
            progress.value++;
        }
        yield return new WaitForSeconds(2);
        info.text = "Finalizing...";
        yield return new WaitForSeconds(2);
        progress.value = progress.maxValue;
        yield return new WaitForSeconds(1);
        player.SetActive(true);
        loadingScreen.SetActive(false);
        yield return new WaitForSeconds(0.25f);
        foreach (Enemy e in allEnemies)
        {
            yield return new WaitForEndOfFrame();
            e.GetTarget();
        }
        timerStarted = true;
    }

    void SetPosition()
    {
        switch (lastAp.nextPos)
        {
            case AvailablePositions.nextPosition.forward:
                ap.myPosition = lastAp.myPosition + new Vector3(0, 1, 0);
                break;
            case AvailablePositions.nextPosition.left:
                ap.myPosition = lastAp.myPosition + new Vector3(-1, 0, 0);
                break;
            case AvailablePositions.nextPosition.right:
                ap.myPosition = lastAp.myPosition + new Vector3(1, 0, 0);
                break;
            default:
                break;
        }
    }

    int lastRandom = -1;
    bool goUp;
    bool goDown;
    int wentUp;
    int wentDown;
    Vector3 RandomPosition()
    {
        int randomPosition = Random.Range(0, 3);
        while (lastRandom == 1 && randomPosition == 2)
        {
            randomPosition = Random.Range(0, 3);
        }
        while (lastRandom == 2 && randomPosition == 1)
        {
            randomPosition = Random.Range(0, 3);
        }
        lastRandom = randomPosition;
        int goUpInt = Random.Range(0, 100);
        int goDownInt = Random.Range(0, 100);
        goUp = false;
        goDown = false;
        if (goUpInt <= goUpChance && goUpChance != 0)
        {
            goUp = true;
        }
        else if (goUpInt <= goDownChance && goDownChance != 0)
        {
            goDown = true;
        }

        Vector3 nextPos = Vector3.zero;
        switch (randomPosition)
        {
            case 0:
                ap.nextPos = AvailablePositions.nextPosition.forward;
                nextPos = ap.forwardPos.position;
                break;
            case 1:
                ap.nextPos = AvailablePositions.nextPosition.left;
                nextPos = ap.leftPos.position;
                break;
            case 2:
                ap.nextPos = AvailablePositions.nextPosition.right;
                nextPos = ap.rightPos.position;
                break;
            default:
                break;
        }
        if (goUp)
        {
            if (wentDown == 1)
            {
                ap.hasHazard = true;
                wentUp = 0;
                wentDown = 0;
            }
            else
            {
                wentUp = 1;
            }
            nextPos += new Vector3(0, 2, 0);
        }
        if (goDown)
        {
            if (wentUp == 1)
            {
                wentDown = 1;
            }
            nextPos += new Vector3(0, -2, 0);
        }
        if (goUp == false && goDown == false)
        {
            wentUp = 0;
            wentDown = 0;
        }
        return nextPos;
    }
    
    bool timerStarted;
    private void Update()
    {
        if (timerStarted)
        {
            timerTime += Time.deltaTime;
            var seconds = Mathf.FloorToInt(timerTime % 60);
            var minutes = Mathf.FloorToInt(timerTime / 60);
            timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
    
}