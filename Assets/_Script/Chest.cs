﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    UIHandler ui;
    public enum ChestRarity {common, rare, legendary }
    bool playerInRange;
    Animator anim;

    public GameObject itemObject;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (playerInRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                anim.Play("Open");
                ui.UpdateSubtitle("");
                FindObjectOfType<MapGenerator>().timerTime -= 2.5f;
                Destroy(this);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (ui == null)
            {
                ui = FindObjectOfType<UIHandler>();
            }
            playerInRange = true;
            ui.UpdateSubtitle("Press E to open");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            ui.UpdateSubtitle("");
        }
    }
}
