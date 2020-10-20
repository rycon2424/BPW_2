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
        pb = FindObjectOfType<PlayerBehaviour>();
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
                Debug.DrawRay(hit.point, Vector3.down * 2f, Color.blue);
                if (!Physics.Raycast(hit.point, Vector3.down, out heightHit, 2f, heightCheck))
                {
                    if (StateMachine.IsInState("Locomotion"))
                    {
                        pb.currentHangpos = hit.point;
                        pb.direction = direction;
                        StateMachine.GoToState(pb, "Hang");
                    }
                }
                else
                {
                    Debug.Log("Player cant stand");
                }
            }
        }
    }

}
