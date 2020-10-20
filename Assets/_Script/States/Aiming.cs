using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aiming : State
{
    public override void OnStateEnter(PlayerBehaviour pb)
    {
        pb.SetCamDistanceAndTarget(2, pb.rangedView);
        pb.crosshair.SetActive(true);
        pb.anim.SetLayerWeight(1, 1);

        pb.bowArm.SetActive(true);
        pb.bowBack.SetActive(false);
        pb.arrowInHand.SetActive(true);
    }

    public override void OnStateExit(PlayerBehaviour pb)
    {
        pb.crosshair.SetActive(false);
    }

    public override void StateUpdate(PlayerBehaviour pb)
    {
        if (Input.GetMouseButton(0))
        {
            StateMachine.GoToState(pb, "Shoot");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StateMachine.GoToState(pb, "Locomotion");
        }
        Movement(pb);
        RotateTowardsCamera(pb);
    }

    public override void StateLateUpdate(PlayerBehaviour pb)
    {
        pb.chest.LookAt(pb.cameraObject.position);
        pb.chest.rotation = pb.chest.rotation * Quaternion.Euler(pb.lookoffset);
    }

    void Movement(PlayerBehaviour pb)
    {
        //pb.anim.SetFloat("Horizontal", Input.GetAxis("Horizontal"));
        pb.anim.SetFloat("Vertical", Input.GetAxis("Vertical"));
    }

    public void RotateTowardsCamera(PlayerBehaviour pb)
    {
        var CharacterRotation = pb.cameraObject.transform.rotation;
        CharacterRotation.x = 0;
        CharacterRotation.z = 0;
        pb.transform.rotation = Quaternion.Slerp(pb.transform.rotation, CharacterRotation, Time.deltaTime * 8);
    }
}
