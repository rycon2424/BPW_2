using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int seed;
    public int mapLength;
    [Range(0, 100)] public int goUpChance;
    [Range(0, 100)] public int goDownChance;
    public Vector3 firstTilePosition;
    [Space]
    public GameObject tile;
    public List<AvailablePositions> allTiles = new List<AvailablePositions>();
    
    void Start()
    {
        UnityEngine.Random.InitState(seed);
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
            OpenCurrentTile();
            
            ap = t.GetComponent<AvailablePositions>();

            lastTile = t;

            lastTile.transform.SetParent(firstTile);

            //yield return new WaitForEndOfFrame();
            
            SetPosition();
            allTiles.Add(ap);
        }
        yield return null;
        StartCoroutine(MakeRooms());
    }

    IEnumerator MakeRooms()
    {
        Debug.Log("make rooms");
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
                    aa.backwardsPos.gameObject.SetActive(false);
                }
            }
            currentPos = a.myPosition;
            currentPos += Vector3.left;
            foreach (AvailablePositions aa in allTiles)
            {
                if (currentPos == aa.myPosition)
                {
                    a.leftPos.gameObject.SetActive(false);
                    aa.rightPos.gameObject.SetActive(false);
                }
            }
        }
    }

    void OpenCurrentTile()
    {
        switch (ap.nextPos)
        {
            case AvailablePositions.nextPosition.forward:
                ap.forwardPos.gameObject.SetActive(false);
                break;
            case AvailablePositions.nextPosition.left:
                ap.leftPos.gameObject.SetActive(false);
                break;
            case AvailablePositions.nextPosition.right:
                ap.rightPos.gameObject.SetActive(false);
                break;
            default:
                break;
        }
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
            nextPos += new Vector3(0, 2, 0);
        }
        if (goDown)
        {
            nextPos += new Vector3(0, -2, 0);
        }
        return nextPos;
    }
}