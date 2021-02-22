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
            //newPlayerPos = -pb.transform.forward * 0.8f;
            right = false;
        }
        else if (pb.PlayerToWall(pb, pb.transform.right * 0.75f, false))
        {
            pb.PlayerFaceWall(pb, -pb.transform.right * 0.25f, pb.transform.right);
            //newPlayerPos = pb.transform.forward * 0.8f;
            right = true;;
        }
        pb.anim.SetBool("WallRunRight", !right);
        pb.anim.SetBool("WallRun", true);
        //pb.transform.position += newPlayerPos;
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
        if (right)
        {
            if (!CheckForWallsRight(pb))
            {
                pb.transform.Rotate(0, -90, 0);
                StateMachine.GoToState(pb, "Falling");
            }
        }
        else
        {
            if (!CheckForWallsLeft(pb))
            {
                pb.transform.Rotate(0, 90, 0);
                StateMachine.GoToState(pb, "Falling");
            }
        }
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

    bool CheckForWallsLeft(PlayerBehaviour pb)
    {
        if (pb.isPlaceToClimb(pb.transform.position + Vector3.up + (-pb.transform.forward * 0.2f), pb.transform.forward, 0.85f))
        {
            if (pb.isPlaceToClimb(pb.transform.position + Vector3.up, pb.transform.right, 1f)
               || pb.isPlaceToClimb(pb.transform.position + Vector3.up / 2, pb.transform.right, 1f)
               || pb.isPlaceToClimb(pb.transform.position + Vector3.up * 1.5f, pb.transform.right, 1f))
            {
                Debug.Log("Hit an wall up front");
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            Debug.Log("No wall left to climb further");
            return false;
        }
    }

    bool CheckForWallsRight(PlayerBehaviour pb)
    {
        if (pb.isPlaceToClimb(pb.transform.position + Vector3.up + (-pb.transform.forward * 0.2f), pb.transform.forward, 0.85f))
        {
            if (pb.isPlaceToClimb(pb.transform.position + Vector3.up, -pb.transform.right, 1f)
              || pb.isPlaceToClimb(pb.transform.position + Vector3.up / 2, -pb.transform.right, 1f)
              || pb.isPlaceToClimb(pb.transform.position + Vector3.up * 1.5f, -pb.transform.right, 1f))
            {
                Debug.Log("Hit an wall up front");
                return false;
            }
            else
            {
                Debug.Log("No wall left to climb further");
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
            ray = new Ray(pb.transform.position + Vector3.up * 1.3f, pb.transform.forward);
            Debug.DrawRay(pb.transform.position + Vector3.up * 1.3f, pb.transform.forward * 1.5f, Color.green);
            if (Physics.Raycast(ray, out hit, 1.5f))
            {
                pb.anim.SetIKPosition(AvatarIKGoal.RightHand, hit.point);
                pb.anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                Debug.Log(hit.collider.name);
            }
            else
            {
                pb.anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            }
        }
        else
        {
            ray = new Ray(pb.transform.position + Vector3.up * 1.3f, pb.transform.forward);
            Debug.DrawRay(pb.transform.position + Vector3.up * 1.3f, pb.transform.forward * 1.5f, Color.green);
            if (Physics.Raycast(ray, out hit, 1.5f))
            {
                pb.anim.SetIKPosition(AvatarIKGoal.LeftHand, hit.point);
                pb.anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                Debug.Log(hit.collider.name);
            }
            else
            {
                pb.anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            }
        }
    }
}
