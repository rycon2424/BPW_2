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
        CheckIfHittingGroundOrWall(pb);
    }

    void CheckIfHittingGroundOrWall(PlayerBehaviour pb)
    {
        RaycastHit groundhit;
        RaycastHit wallFhit;
        RaycastHit wallBhit;

        Vector3 calculatedV = pb.transform.position + new Vector3(0, 1.5f, 0);

        Debug.DrawRay(calculatedV, Vector3.down * 1.25f, Color.blue, 0.25f);
        Debug.DrawRay(calculatedV, pb.transform.forward * 0.5f, Color.blue, 0.25f);
        Debug.DrawRay(calculatedV, pb.transform.forward * -0.5f, Color.blue, 0.25f);

        if (Physics.Raycast(calculatedV, Vector3.down, out groundhit , 1.25f, pb.hitGround)) // Check if hitting ground
        { LetGo(pb); Debug.Log("GroundCheck has hit" + groundhit.collider.name); }
        if (Physics.Raycast(calculatedV, pb.transform.forward, out wallFhit, 0.5f, pb.hitGround)) // if hitting wall forward
        { LetGo(pb); Debug.Log("FrontCheck has hit" + wallFhit.collider.name); }
        if (Physics.Raycast(calculatedV, -pb.transform.forward, out wallBhit, 0.5f, pb.hitGround)) // if hitting wall back
        { LetGo(pb); Debug.Log("BackCheck has hit" + wallBhit.collider.name); }
    }

    void LetGo(PlayerBehaviour pb)
    {
        pb.anim.SetTrigger("Hang");
        StateMachine.GoToState(pb, "Locomotion");
    }

}
