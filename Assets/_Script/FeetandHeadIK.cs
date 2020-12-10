using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeetandHeadIK : MonoBehaviour
{
    [Range(0, 1)] public float IKStrength;
    public Transform headLook;
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }
    
    void OnAnimatorIK()
    {
        anim.SetLookAtWeight(IKStrength);
        anim.SetLookAtPosition(headLook.position);
    }
}
