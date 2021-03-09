using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int playerMaxHp;
    public Vector3 shotgunAccuracy;
    public float shootAttackSpeed = 1;
    public float meleeAttackSpeed = 1;
    public float runSpeed = 1;
    public float climbSpeed = 1;

    PlayerBehaviour pb;

    void Start()
    {
        pb = GetComponent<PlayerBehaviour>();
    }
    
    void Update()
    {
        if (pb.anim.GetBool("Sprinting"))
        {
            pb.anim.speed = runSpeed;
            return;
        }
        if (pb.pc.inCombo)
        {
            pb.anim.speed = meleeAttackSpeed;
            return;
        }
        pb.anim.speed = 1;
    }
}
