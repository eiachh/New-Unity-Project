﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    //self as the object with this hitbox not the actual hitbox
    [Tooltip("The reference to the Interractable script (the parents)")]
    public Interractable self;


    Character_Script MainCharacter;

    //adds the reference when the maincharacter entered the hitbox
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "mainchar" && other != null)
        {
            MainCharacter = other.gameObject.GetComponent<Character_Script>();
            //passes the parents Interractable script for the player for further interraction at "F" press
            MainCharacter.addForInterraction(self);


        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "mainchar" && other != null)
        {
            MainCharacter = other.gameObject.GetComponent<Character_Script>();
            MainCharacter.removeForInterraction(self);
        }
    }
}
