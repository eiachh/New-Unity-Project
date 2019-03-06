using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battle_Capability_Handler : MonoBehaviour
{
    [Tooltip("This is the default health without buffs or anything")]
    public int BaseHealth = 10;
    public int CurrentHealth = 10;
    [Tooltip("Its a placeholder for mana or whatever characters will have")]
    public int BaseManaLikeStat = 10;
    public int CurrentManaLikeStat = 10;
    //if base speed is too high it can result in weird turnorders Never go over 50
    [Tooltip("The calculation caps at 100 so if you set this too high characters can have multiple turns")]
    public int baseSpeed = 10;
    //will be added to the baseSpeed
    [Tooltip("This is modifiers that items or buffs can set its added to baseSpeed")]
    public int speedModifier = 0;
    public int currentSpeed=0;

    [Tooltip("Used to determine if the character is alive. If its set to false and the character is added to the party it will be possible to revieve it(probably)")]
    public bool isAlive = true;

    //use lowercase
    [Tooltip("Has to decide if it's not set the character will be ignored! Choose one of the options!")]
    public string typeOfBattler = "player/enemy";
    


    public string SkillListPlaceholder="ideglenesen kell a fight hud csinalasahoz";
}
