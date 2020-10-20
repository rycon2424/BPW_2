using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : State
{
    GameObject ob;
    Arrow ar;

    public override void OnStateEnter(PlayerBehaviour pb)
    {
        pb.arrowInHand.SetActive(false);
        pb.oc.gameObject.SetActive(false);

        ob = Object.Instantiate(pb.arrow, pb.arrowSpawn.transform.position, pb.arrowSpawn.transform.rotation);
        ar = ob.GetComponent<Arrow>();
        ar.pb = pb;

        Time.timeScale = pb.slowTimeSpeed;
        pb.anim.speed = 0;
        //pb.GetComponent<AudioListener>().enabled = false;
    }

    public override void OnStateExit(PlayerBehaviour pb)
    {
        pb.anim.speed = 1;
        Time.timeScale = 1;
        pb.oc.gameObject.SetActive(true);
        //pb.GetComponent<AudioListener>().enabled = true;
    }

    public override void StateUpdate(PlayerBehaviour pb)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            Time.timeScale = pb.slowTimeSpeed * 2;
        }
        else
        {
            Time.timeScale = pb.slowTimeSpeed;
        }
    }

    public override void StateLateUpdate(PlayerBehaviour pb)
    {
        pb.chest.LookAt(pb.cameraObject.position);
        pb.chest.rotation = pb.chest.rotation * Quaternion.Euler(pb.lookoffset);
    }

}
