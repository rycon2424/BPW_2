using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ItemDataBase
{
    public string name;
    public Sprite icon;
    public Color colorLight;
    public string description;
    [Space]
    public int pellets;
    public Vector3 acurracy;
    public float attackSpeed;
    public float shootSpeed;
    public float runSpeed;
    public float climbSpeed;
    public int meleeDamage;
    public int shootDamage;
}
