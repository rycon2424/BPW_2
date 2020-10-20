using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pole : MonoBehaviour
{
    public PoleHitBoxes[] allHitBoxes;
    public List<Vector3> hitLocations = new List<Vector3>();

    void Start()
    {
        foreach (var pole in allHitBoxes)
        {
            pole.parentPole = this;
        }
    }

    public void UpdatePole()
    {
        foreach (var pole in allHitBoxes)
        {
            if (pole.hit == false)
            {
                return;
            }
        }
        Arrow ar = FindObjectOfType<Arrow>();
        ar.Succes(hitLocations);
    }
}
