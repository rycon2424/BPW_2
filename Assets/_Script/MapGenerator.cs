﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public int seed;
    public int mapLength;
    public Vector3 firstTilePosition;
    [Space]
    public GameObject tile;

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

            yield return new WaitForEndOfFrame();

            //Debug.Log("Current Tile = " + ap.gameObject.name +" Last tile was " + lastAp.gameObject.name);
            OpenToLastTile();
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

    void OpenToLastTile()
    {
        switch (lastAp.nextPos)
        {
            case AvailablePositions.nextPosition.forward:
                ap.backwardsPos.gameObject.SetActive(false);
                break;
            case AvailablePositions.nextPosition.left:
                ap.rightPos.gameObject.SetActive(false);
                break;
            case AvailablePositions.nextPosition.right:
                ap.leftPos.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

    int lastRandom = -1;
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
        return nextPos;
    }
}