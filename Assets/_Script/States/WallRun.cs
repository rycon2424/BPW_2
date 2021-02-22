using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : State
{
    public bool right;
    public override void OnStateEnter(PlayerBehaviour pb)
    {
        pb.anim.applyRootMotion = false;
        wallClimb = 1;
        //pb.characterController.enabled = false;
        Vector3 newPlayerPos = Vector3.zero;
        if (pb.PlayerToWall(pb, -pb.transform.right * 0.75f, false))
        {
            pb.PlayerFaceWall(pb, pb.transform.right * 0.25f ,-pb.transform.right);
            newPlayerPos = -pb.transform.forward * 0.8f;
            right = false;
        }
        else if (pb.PlayerToWall(pb, pb.transform.right * 0.75f, false))
        {
            pb.PlayerFaceWall(pb, -pb.transform.right * 0.25f, pb.transform.right);
            newPlayerPos = pb.transform.forward * 0.8f;
            right = true;;
        }
        pb.anim.SetBool("WallRunRight", !right);
        pb.anim.SetBool("WallRun", true);
        pb.transform.position += newPlayerPos;
    }

    public override void OnStateExit(PlayerBehaviour pb)
    {
        pb.anim.applyRootMotion = true;
        pb.anim.SetBool("WallRun", false);
    }

    public override void StateLateUpdate(PlayerBehaviour pb)
    {
        wallClimb -= 0.005f;
    }

    float wallClimb;
    public override void StateUpdate(PlayerBehaviour pb)
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            StateMachine.GoToState(pb, "Falling");
        }
        Vector3 dir = Vector3.zero;
        if (pb.anim.GetBool("WallRunRight"))
        {
            dir = pb.transform.right * Time.deltaTime * 4;
        }
        else
        {
            dir = -pb.transform.right * Time.deltaTime * 4;
        }
        dir += Vector3.up * wallClimb * Time.deltaTime;
        pb.characterController.Move(dir);
    }
}
