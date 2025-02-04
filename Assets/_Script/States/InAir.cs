﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAir : State
{
    public override void OnStateEnter(PlayerBehaviour pb)
    {
        pb.airTime = 0;
        airTimePos = pb.transform.position;
    }

    public override void OnStateExit(PlayerBehaviour pb)
    {
        pb.pc.inCombo = false;
        pb.anim.SetBool("Combo", false);
    }

    public override void StateLateUpdate(PlayerBehaviour pb)
    {

    }

    Vector3 airTimePos;
    public override void StateUpdate(PlayerBehaviour pb)
    {
        pb.airTime = Mathf.RoundToInt(Vector3.Distance(pb.transform.position, airTimePos));
        if (Input.GetMouseButtonDown(0) && !pb.inJumpAttack)
        {
            pb.jumped = false;
            pb.inJumpAttack = true;
            pb.anim.SetTrigger("JumpAttack");
            airTimePos = pb.transform.position;
        }
        Debug.ClearDeveloperConsole();
        if (pb.airTime >= 7)
        {
            StateMachine.GoToState(pb, "Falling");
            return;
        }
        if (pb.isPlaceToClimb(pb.transform.position + Vector3.up * 0.5f, Vector3.down, 0.5f))
        {
            StateMachine.GoToState(pb, "Locomotion");
            return;
        }
        if (pb.jumped)
        {
            if (Input.GetKey(pb.kc.grab))
            {
                pb.FindLedge(pb);
            }
        }
        if (pb.jumped && pb.canWallRun)
        {
            Vector3 fallSpeed = pb.transform.up * Time.deltaTime * -3;
            pb.characterController.Move(fallSpeed);
            if (Input.GetKey(pb.kc.jump))
            {
                if (pb.isPlaceToClimb(pb.transform.position + Vector3.up, pb.transform.forward, 0.5f))
                {
                    if (pb.wallRunState == 1)
                    {
                        pb.wallRunState = 2;
                        pb.transform.Rotate(0, 90, 0);
                    }
                    else
                    {
                        pb.wallRunState = 1;
                        pb.transform.Rotate(0, -90, 0);
                    }
                    StateMachine.GoToState(pb, "WallRun");
                }
            }
        }
    }

    public override void AnimatorIKUpdate(PlayerBehaviour pb)
    {
    }

}
