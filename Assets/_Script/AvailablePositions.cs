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
}
