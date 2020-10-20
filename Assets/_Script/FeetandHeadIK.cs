using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetandHeadIK : MonoBehaviour
{
    public Transform headLook;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    void OnAnimatorIK()
    {
        anim.SetLookAtWeight(0.35f);
        anim.SetLookAtPosition(headLook.position);
    }
}
