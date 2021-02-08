using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public bool inCombo;
    [Space]
    PlayerBehaviour pb;
    public GameObject swordHand;
    public GameObject swordBack;
    
    private void Start()
    {
        pb = GetComponent<PlayerBehaviour>();
    }

    public void CombatUpdate()
    {
        if (Input.GetMouseButton(1) && !inCombo)
        {
            RangedUpdate();
            pb.crossHairUI.SetActive(true);
        }
        else
        {
            MeleeUpdate();
            pb.crossHairUI.SetActive(false);
        }
    }

    void RangedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Shoot");
        }
    }

    void MeleeUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (inCombo == false)
            {
                inCombo = true;
                pb.anim.SetBool("Combo", true);
                swordBack.SetActive(false);
                swordHand.SetActive(true);
            }
            else if (inCombo == true)
            {
                pb.anim.SetBool("Attack", true);
            }
        }
    }
    
    public void InteruptAttack()
    {
        inCombo = false;
        pb.anim.SetBool("Combo", false);
        pb.anim.SetBool("Attack", false);
        swordBack.SetActive(true);
        swordHand.SetActive(false);
        Debug.Log("Reset");
    }

    public void WaitForNextAttack()
    {
        pb.anim.SetBool("Attack", false);
    }

    public void ResetEverything()
    {
        if (!pb.anim.GetBool("Attack"))
        {
            InteruptAttack();
        }
    }
}
