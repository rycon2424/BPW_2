using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hanging : State
{
    public override void OnStateEnter(PlayerBehaviour pb)
    {
        pb.anim.applyRootMotion = false;
        pb.characterController.enabled = false;
        Vector3 newPlayerPos = pb.transform.position + pb.hangOffset;
        newPlayerPos.y += pb.grabHeight;
        
        pb.transform.position = newPlayerPos;

        if (!pb.PlayerFaceWall(pb))
        {
            StateMachine.GoToState(pb, "Falling");
            return;
        }
        if (!pb.PlayerToWall(pb))
        {
            StateMachine.GoToState(pb, "Falling");
            return;
        }
        pb.anim.SetTrigger("Hang");

        pb.DelayTurnOnRoot(0.25f);
    }

    public override void OnStateExit(PlayerBehaviour pb)
    {
        pb.fallDuration = 0;
    }
    
    public override void StateUpdate(PlayerBehaviour pb)
    {
        Vector3 offset = (pb.transform.up * 1.25f) + (pb.transform.forward * 0.2f);
        Vector3 startPos = pb.transform.position - (pb.transform.forward * 0.1f);
        if (Input.GetKey(KeyCode.D))
        {
            if (!isPlaceToClimb(startPos, pb.transform.right, 1f) && !isPlaceToClimb(startPos + offset, pb.transform.right, 1f))
            {
                if (isPlaceToClimb(startPos + (pb.transform.right * 0.75f), pb.transform.forward, 0.5f))
                {
                    pb.horizontal = Input.GetAxis("Horizontal");
                }
            }
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (!isPlaceToClimb(startPos, -pb.transform.right, 1f) && !isPlaceToClimb(startPos + offset, -pb.transform.right, 1f))
            {
                if (isPlaceToClimb(startPos + (-pb.transform.right * 0.75f), pb.transform.forward, 0.5f))
                {
                    pb.horizontal = Input.GetAxis("Horizontal");
                }
            }
        }
        pb.anim.SetFloat("Horizontal", pb.horizontal);

        if (pb.horizontal < 0)
        {
            pb.horizontal += 1 * Time.deltaTime;
        }
        if (pb.horizontal > 0)
        {
            pb.horizontal -= 1 * Time.deltaTime;
        }
        if (Mathf.Abs(pb.horizontal) < 0.1f)
        {
            pb.horizontal = 0;
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            StateMachine.GoToState(pb, "Falling");
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            if (IsRoomToClimb(pb))
            {
                pb.anim.SetTrigger("Jump");
                StateMachine.GoToState(pb, "StateBetween");
            }
        }
    }
    
    bool IsRoomToClimb(PlayerBehaviour pb)
    {
        Vector3 offset = pb.transform.position + (pb.transform.forward * 0.75f) + Vector3.up;
        RaycastHit hit;
        Ray ray = new Ray(offset, Vector3.up);
        Debug.DrawRay(offset, Vector3.up * 2, Color.yellow, 1);
        if (Physics.Raycast(ray, out hit, 2))
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    bool isPlaceToClimb(Vector3 start, Vector3 dir, float length)
    {
        RaycastHit hit;
        Ray ray = new Ray(start, dir);
        Debug.DrawRay(start, dir * length, Color.blue, 0.1f);
        if (Physics.Raycast(ray, out hit, length))
        {
            Debug.Log(hit.collider.name);
            return true;
        }
        else
        {
            return false;
        }
    }

    public override void StateLateUpdate(PlayerBehaviour pb)
    {

    }
}
