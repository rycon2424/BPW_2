using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRun : State
{
    public bool right;
    bool closeToWall;
    bool foundWall;
    public override void OnStateEnter(PlayerBehaviour pb)
    {
        pb.anim.applyRootMotion = false;
        wallClimb = 1;
        Vector3 newPlayerPos = Vector3.zero;
        if (pb.PlayerToWall(pb, -pb.transform.right * 0.75f, false))
        {
            pb.PlayerFaceWall(pb, pb.transform.right * 0.25f ,-pb.transform.right);
            pb.transform.Rotate(0, 90, 0);
            right = false;
            foundWall = true;
        }
        else if (pb.PlayerToWall(pb, pb.transform.right * 0.75f, false))
        {
            pb.PlayerFaceWall(pb, -pb.transform.right * 0.25f, pb.transform.right);
            pb.transform.Rotate(0, -90, 0);
            right = true;
            foundWall = true;
        }
        else
        {
            foundWall = false;
        }
        if (foundWall == false)
        {
            Debug.Log("Hit no wall");
            StateMachine.GoToState(pb, "Falling");
            return;
        }
        pb.anim.SetBool("WallRunRight", !right);
        pb.anim.SetBool("WallRun", true);
        pb.anim.SetTrigger("WRunNow");
        closeToWall = true;
    }

    public override void OnStateExit(PlayerBehaviour pb)
    {
        pb.anim.applyRootMotion = true;
        pb.anim.SetBool("WallRun", false);
    }

    public override void StateLateUpdate(PlayerBehaviour pb)
    {
        wallClimb -= 0.01f;
    }

    float wallClimb;
    public override void StateUpdate(PlayerBehaviour pb)
    {
        if (Input.GetKeyDown(pb.kc.jump))
        {
            pb.anim.SetTrigger("Jump");
            if (right)
            {
                pb.transform.Rotate(0, -90, 0);
            }
            else
            {
                pb.transform.Rotate(0, 90, 0);
            }
            pb.jumped = true;
            StateMachine.GoToState(pb, "InAir");
            return;
        }
        if (pb.isPlaceToClimb(pb.transform.position + Vector3.up * 0.5f, Vector3.down, 0.5f))
        {
            StateMachine.GoToState(pb, "Locomotion");
            return;
        }
        if (right)
        {
            if (!CheckForWallsRight(pb))
            {
                if (pb.grounded)
                {
                    StateMachine.GoToState(pb, "Locomotion");
                }
                else
                {
                    StateMachine.GoToState(pb, "Falling");
                }
            }
        }
        else
        {
            if (!CheckForWallsLeft(pb))
            {
                if (pb.grounded)
                {
                    StateMachine.GoToState(pb, "Locomotion");
                }
                else
                {
                    StateMachine.GoToState(pb, "Falling");
                }
            }
        }
        if (Input.GetKeyDown(pb.kc.drop))
        {
            StateMachine.GoToState(pb, "Falling");
        }
        Vector3 dir = Vector3.zero;
        dir = pb.transform.forward * Time.deltaTime * 5 + TooCloseToWall(pb);
        dir += Vector3.up * wallClimb * Time.deltaTime;
        //Debug.Log(dir);
        pb.characterController.Move(dir);
    }

    Vector3 TooCloseToWall(PlayerBehaviour pb)
    {
        if (closeToWall)
        {
            Vector3 startPoint = pb.transform.position + Vector3.up * 0.8f;
            if (right)
            {
                if (pb.isPlaceToClimb(startPoint, pb.transform.right, 0.5f))
                {
                    return -pb.transform.right * 0.05f;
                }
                else
                {
                    return Vector3.zero;
                }
            }
            else
            {
                if (pb.isPlaceToClimb(startPoint, -pb.transform.right, 0.5f))
                {
                    return pb.transform.right * 0.05f;
                }
                else
                {
                    return Vector3.zero;
                }
            }
        }
        return Vector3.zero;
    }

    bool CheckForWallsLeft(PlayerBehaviour pb)
    {
        if (pb.isPlaceToClimb(pb.transform.position + Vector3.up + (pb.transform.right * 0.2f), -pb.transform.right, 1.25f))
        {
            if (pb.isPlaceToClimb(pb.transform.position + Vector3.up, pb.transform.forward, 1f)
               || pb.isPlaceToClimb(pb.transform.position + Vector3.up / 2, pb.transform.forward, 1f)
               || pb.isPlaceToClimb(pb.transform.position + Vector3.up * 1.5f, pb.transform.forward, 1f))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    bool CheckForWallsRight(PlayerBehaviour pb)
    {
        if (pb.isPlaceToClimb(pb.transform.position + Vector3.up + (-pb.transform.right * 0.2f), pb.transform.right, 0.85f))
        {
            if (pb.isPlaceToClimb(pb.transform.position + Vector3.up, pb.transform.forward, 1f)
              || pb.isPlaceToClimb(pb.transform.position + Vector3.up / 2, pb.transform.forward, 1f)
              || pb.isPlaceToClimb(pb.transform.position + Vector3.up * 1.5f, pb.transform.forward, 1f))
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }

    public override void AnimatorIKUpdate(PlayerBehaviour pb)
    {
        RaycastHit hit;
        Ray ray;
        if (right)
        {
            ray = new Ray(pb.transform.position + Vector3.up * 1.3f, pb.transform.right);
            Debug.DrawRay(pb.transform.position + Vector3.up * 1.3f, pb.transform.right * 1.5f, Color.green);
            if (Physics.Raycast(ray, out hit, 1.5f))
            {
                pb.anim.SetIKPosition(AvatarIKGoal.RightHand, hit.point);
                pb.anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
            }
            else
            {
                pb.anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            }
        }
        else
        {
            ray = new Ray(pb.transform.position + Vector3.up * 1.3f, -pb.transform.right);
            Debug.DrawRay(pb.transform.position + Vector3.up * 1.3f, -pb.transform.right * 1.5f, Color.green);
            if (Physics.Raycast(ray, out hit, 1.5f))
            {
                pb.anim.SetIKPosition(AvatarIKGoal.LeftHand, hit.point);
                pb.anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
            }
            else
            {
                pb.anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            }
        }
    }
}
