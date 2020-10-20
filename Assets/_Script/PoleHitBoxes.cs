using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleHitBoxes : MonoBehaviour
{
    [HideInInspector] public Pole parentPole;
    public bool hit;
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Arrow"))
        {
            hit = true;
            Arrow ar = other.GetComponent<Arrow>();
            //parentPole.hitLocations.Add(ar.transform.position);
            parentPole.hitLocations.Add(transform.position);
            parentPole.UpdatePole();

            GetComponent<Collider>().enabled = false;
        }
    }
}
