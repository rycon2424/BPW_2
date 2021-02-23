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
        if (pb.jumped && pb.canWallRun)
        {
            if (Input.GetKey(pb.kc.jump))
            {
                pb.TryWallRun();
            }
        }
    }

    public override void AnimatorIKUpdate(PlayerBehaviour pb)
    {
    }

}
