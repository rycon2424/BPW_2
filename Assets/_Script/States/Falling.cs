using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : State
{
    public override void OnStateEnter(PlayerBehaviour pb)
    {
        pb.characterController.enabled = true;
        pb.anim.SetTrigger("Falling");
    }

    public override void OnStateExit(PlayerBehaviour pb)
    {

    }

    public override void StateUpdate(PlayerBehaviour pb)
    {
        if (pb.grounded)
        {
            StateMachine.GoToState(pb, "Locomotion");
        }
    }

    public override void StateLateUpdate(PlayerBehaviour pb)
    {

    }
}
