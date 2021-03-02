using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : State
{

    public override void OnStateEnter(PlayerBehaviour pb)
    {
        pb.anim.applyRootMotion = true;
        pb.characterController.enabled = true;
        pb.anim.SetTrigger("Falling");
        pb.anim.SetInteger("FallType", -1);
    }

    public override void OnStateExit(PlayerBehaviour pb)
    {

    }

    public override void StateUpdate(PlayerBehaviour pb)
    {
        if (pb.grounded)
        {
            Debug.Log(Mathf.RoundToInt(Vector3.Distance(pb.noGroundPosition, pb.transform.position)));
            pb.fallDistance = Mathf.RoundToInt(Vector3.Distance(pb.noGroundPosition, pb.transform.position));
            if (pb.fallDistance <= 10)
            {
                pb.anim.SetInteger("FallType", 0);
                StateMachine.GoToState(pb, "Locomotion");
            }
            else if (pb.fallDistance >= 11)
            {
                pb.anim.SetInteger("FallType", 1);
                StateMachine.GoToState(pb, "Locomotion");
            }
        }
    }

    public override void StateLateUpdate(PlayerBehaviour pb)
    {

    }
}
