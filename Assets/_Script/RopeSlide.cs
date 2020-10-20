using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSlide : MonoBehaviour
{

    LineRenderer lr;
    public Vector3 direction;
    public LayerMask heightCheck;
    PlayerBehaviour pb;

    void Start()
    {
        lr = GetComponent<LineRenderer>();
        direction = (lr.GetPosition(1) - lr.GetPosition(0)).normalized;
    }
    
    void Update()
    {
        CheckPlayer();
    }

    void CheckPlayer()
    {
        RaycastHit hit;

        float range = Vector3.Distance(lr.GetPosition(1), lr.GetPosition(0));

        Debug.DrawRay(transform.position, direction * range, Color.red);

        if (Physics.Raycast(transform.position, direction, out hit, range))
        {
            if (hit.collider.CompareTag("Player"))
            {
                RaycastHit heightHit;
                Debug.DrawRay(hit.point, Vector3.down * 1.8f, Color.blue);
                if (!Physics.Raycast(hit.point, Vector3.down, out heightHit, 1.8f, heightCheck))
                {
                    Debug.Log("Player can Enter");
                }
                else
                {
                    Debug.Log("Player cant stand");
                }
            }
        }
    }

}
