﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hanging : State
{
    public override void OnStateEnter(PlayerBehaviour pb)
    {
        pb.anim.applyRootMotion = false;
        pb.characterController.enabled = false;
        pb.anim.SetTrigger("Hang");
        Vector3 newPlayerPos = pb.transform.position + pb.hangOffset;
        newPlayerPos.y += pb.grabHeight;
        pb.transform.position = newPlayerPos;

        PlayerFaceWall(pb);
        PlayerToWall(pb);
        
        pb.transform.position -= pb.transform.forward * 0.1f;

        pb.DelayTurnOnRoot(0.25f);
    }

    void PlayerFaceWall(PlayerBehaviour pb)
    {
        RaycastHit hit;
        float range = 2;
        Vector3 playerHeight = new Vector3(pb.transform.position.x, pb.transform.position.y + 1f, pb.transform.position.z);
        Debug.DrawRay(playerHeight, pb.transform.forward * range, Color.cyan, 5);
        if (Physics.Raycast(playerHeight, pb.transform.forward, out hit, range))
        {
            pb.transform.rotation = Quaternion.LookRotation(-hit.normal, Vector3.up);
        }
    }

    void PlayerToWall(PlayerBehaviour pb)
    {
        RaycastHit hit;
        float range = 2;
        Vector3 playerHeight = new Vector3(pb.transform.position.x, pb.transform.position.y + 1f, pb.transform.position.z);
        Debug.DrawRay(playerHeight, pb.transform.forward * range, Color.yellow, 5);
        if (Physics.Raycast(playerHeight, pb.transform.forward, out hit, range))
        {
            //Debug.Log("Distance is " + (pb.transform.position - hit.point));
            Vector3 temp = pb.transform.position - hit.point;
            temp.y = 0;
            pb.transform.position -= temp;
        }
    }

    public override void OnStateExit(PlayerBehaviour pb)
    {

    }

    public override void StateUpdate(PlayerBehaviour pb)
    {
        pb.anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        if (Input.GetKeyDown(KeyCode.X))
        {
            StateMachine.GoToState(pb, "Falling");
        }
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            pb.anim.SetTrigger("Jump");
            StateMachine.GoToState(pb, "StateBetween");
        }
    }

    public override void StateLateUpdate(PlayerBehaviour pb)
    {
    }
}
