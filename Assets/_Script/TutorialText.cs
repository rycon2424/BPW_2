using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialText : MonoBehaviour
{
    public float distance;
    public Transform player;

    public GameObject ob;

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < distance)
        {
            ob.SetActive(true);
        }
        else
        {
            ob.SetActive(false);
        }
    }
}
