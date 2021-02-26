using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : State
{
    public override void OnStateEnter(PlayerBehaviour pb)
    {
        pb.transform.rotation = Quaternion.LookRotation(pb.CalculateSlopeDirection());
    }

    public override void OnStateExit(PlayerBehaviour pb)
    {

    }

    public override void StateUpdate(PlayerBehaviour pb)
    {

    }
    
    public override void StateLateUpdate(PlayerBehaviour pb)
    {

    }
    
    public override void AnimatorIKUpdate(PlayerBehaviour pb)
    {

    }

}
