using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Actor : MonoBehaviour
{
    [Header("Stats")]
    public bool dead;
    public int hp;
    public Slider hpBar;

    //Called in start
    public void SetupHealth()
    {
        hpBar.value = hp;
    }

    public virtual void TakeDamage(int damage)
    {
        if (dead)
        {
            return;
        }
        hp -= damage;
        hpBar.value = hp;
        if (hp <= 0)
        {
            dead = true;
            OnDeath();
        }
    }

    public virtual void OnDeath() { }

}
