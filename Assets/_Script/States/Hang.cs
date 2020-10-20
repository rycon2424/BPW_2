using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hang : State
{
    public override void OnStateEnter(PlayerBehaviour pb)
    {
        pb.anim.applyRootMotion = false;

        pb.anim.SetTrigger("Hang");
        pb.transform.position = pb.currentHangpos + pb.hangOffset;

        pb.bowBack.SetActive(false);
        pb.bowArm.SetActive(true);

    }

    public override void OnStateExit(PlayerBehaviour pb)
    {
        pb.anim.applyRootMotion = true;

        pb.bowBack.SetActive(true);
        pb.bowArm.SetActive(false);

        pb.canJump = true;
    }

    public override void StateUpdate(PlayerBehaviour pb)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            pb.anim.SetTrigger("Hang");
            StateMachine.GoToState(pb, "Locomotion");
        }
        pb.transform.position += (pb.direction * Input.GetAxis("Vertical")) * Time.deltaTime * 3;
        CheckIfHittingGround(pb);
    }

    void CheckIfHittingGround(PlayerBehaviour pb)
    {
        RaycastHit hit;
        Vector3 calculatedV = pb.transform.position + new Vector3(0, 1.5f, 0);
        Debug.DrawRay(calculatedV, Vector3.down * 1.25f, Color.blue, 0.25f);
        if (Physics.Raycast(calculatedV, Vector3.down, out hit , 1.25f, pb.hitGround))
        {
            pb.anim.SetTrigger("Hang");
            StateMachine.GoToState(pb, "Locomotion");
            Debug.Log(hit.collider.name);
        }
    }

}
