using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public string itemName;
    public MapGenerator mg;
    ItemDataBase item;

    public void Setup(ItemDataBase _item)
    {
        item = _item;
        itemName = _item.name;
        GetComponent<Light>().color = _item.colorLight;
        mg = FindObjectOfType<MapGenerator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerStats ps = other.GetComponent<PlayerStats>();
        if (ps != null)
        {
            GivePlayer(ps);
        }
    }

    void GivePlayer(PlayerStats ps)
    {
        //Gun related;
        ps.pellets += item.pellets;
        ps.shootAttackSpeed += item.shootSpeed;
        ps.shootDamage += item.shootDamage;

        //Melee related;
        ps.meleeAttackSpeed += item.attackSpeed;
        ps.meleeDamage += item.meleeDamage;

        //Locomotion related;
        ps.runSpeed += item.runSpeed;
        ps.climbSpeed += item.climbSpeed;

        mg.ShowReceivedItem(item);

        Destroy(gameObject);
    }

}
