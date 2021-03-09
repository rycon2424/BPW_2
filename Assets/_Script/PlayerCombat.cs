﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : Actor
{
    public bool inCombo;
    public bool shootCld;
    [Space]
    PlayerBehaviour pb;
    public GameObject[] swordHand;
    public GameObject[] swordBack;
    [Space]
    public List<Enemy> enemies = new List<Enemy>();

    private void Start()
    {
        pb = GetComponent<PlayerBehaviour>();
    }

    public void CombatUpdate()
    {
        if (Input.GetMouseButton(1) && !inCombo && pb.Aiming)
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
        if (shootCld)
        {
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            shootCld = true;
            Debug.Log("Shoot");
            Invoke("ShootCooldown", pb.st.shootAttackSpeed);
        }
    }

    void ShootCooldown()
    {
        shootCld = false;
    }

    void MeleeUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (inCombo == false)
            {
                inCombo = true;
                pb.anim.SetBool("Combo", true);
                ShowWeapon();
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
    }

    public void ShowWeapon()
    {
        foreach (var item in swordBack)
        {
            item.SetActive(false);
        }
        foreach (var item in swordHand)
        {
            item.SetActive(true);
        }
    }

    public void Sheat()
    {
        foreach (var item in swordBack)
        {
            item.SetActive(true);
        }
        foreach (var item in swordHand)
        {
            item.SetActive(false);
        }
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

    public void DealDamage()
    {
        if (enemies.Count > 0)
        {
            foreach (Enemy e in enemies)
            {
                e.TakeDamage(25);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy e = other.GetComponent<Enemy>();
        if (e != null)
        {
            enemies.Add(e);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Enemy e = other.GetComponent<Enemy>();
        if (e != null)
        {
            enemies.Remove(e);
        }
    }
}
