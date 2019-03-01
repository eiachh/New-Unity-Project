using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Capability_Handler : MonoBehaviour
{
    public int BaseHealth = 10;
    public int CurrentHealth = 10;
    public int BaseManaLikeStat = 10;
    public int CurrentManaLikeStat = 10;
    //if base speed is too high it can result in weird turnorders Never go over 50
    public int baseSpeed = 10;
    //will be added to the baseSpeed
    public int speedModifier = 0;
    public int currentSpeed=0;

    public bool isAlive = true;

    //use lowercase
    public string typeOfBattler = "player/enemy";

    public string SkillListPlaceholder="ideglenesen kell a fight hud csinalasahoz";
}
