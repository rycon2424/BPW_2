using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InAir : State
{
    public override void OnStateEnter(PlayerBehaviour pb)
    {
    }

    public override void OnStateExit(PlayerBehaviour pb)
    {
    }

    public override void StateLateUpdate(PlayerBehaviour pb)
    {
    }

    public override void StateUpdate(PlayerBehaviour pb)
    {
        Vector3 fallSpeed = pb.transform.up * Time.deltaTime * -3;
        pb.characterController.Move(fallSpeed);
        if (Input.GetMouseButtonDown(0) && !pb.inJumpAttack)
        {
            pb.jumped = false;
            pb.inJumpAttack = true;
            pb.anim.SetTrigger("JumpAttack");
        }
        if (pb.isPlaceToClimb(pb.transform.position + Vector3.up * 0.5f, Vector3.down, 0.5f))
        {
            StateMachine.GoToState(pb, "Locomotion");
            return;
        }
        if (Input.GetKey(KeyCode.E))
        {
            pb.FindLedge(pb);
        }
        if (pb.jumped && pb.canWallRun)
        {
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
