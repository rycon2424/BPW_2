using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvailablePositions : MonoBehaviour
{
    public nextPosition nextPos;
    public enum nextPosition { forward, left, right}
    [Space]
    public Transform forwardPos;
    public Transform leftPos;
    public Transform rightPos;
    public Transform backwardsPos;
    [Space]
    public Vector3 myPosition;
    [Space]
    public bool hasWallF;
    public bool hasWallL;
    public bool hasWallR;
    public bool hasWallB;
    [Space]
    public bool hasObject;
    [Space]
    public bool hasHazard;
    public GameObject[] hazard;
    [Space]
    public Transform[] spawnLocations;
    [Space]
    public GameObject endPortal;
}
