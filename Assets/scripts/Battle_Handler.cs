using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Battle_Handler : MonoBehaviour
{
    public delegate void BattleStartedEventHandler(object sender, EventArgs e);
    public event BattleStartedEventHandler BattleStarted;

    public delegate void BattleFinishedEventHandler(object sender, EventArgs e);
    public event BattleFinishedEventHandler BattleFinished;

    public delegate void DamageRecievedEventHandler(object sender, EventArgs e);
    public event DamageRecievedEventHandler DamageRecieved;

    public Canvas BattleUI;
    public Text HpDisplayingText;

    List<User_Battle_Unit> friendlies;
    List<Enemy_Base> enemies;

    Battle_Capability_Handler activeCharacter = null;

    int speedCap = 100;
    //counts back till the next turn can start it is happening so the user can see the hud changes turnEnding() uses it
    float countBackForNextTurnStart = -1;


    void Update()
    {
        if (countBackForNextTurnStart > 0)
        {
            countBackForNextTurnStart -= Time.deltaTime;
        }
        //FIX THIS REEEE
        if (countBackForNextTurnStart <= 0)
        {
            nextTurn();
        }
    }

   public void initiateBattle(List<User_Battle_Unit> _friendlies,List<Enemy_Base> _enemies)
    {


        BattleUI.enabled = true;
        friendlies = _friendlies;
        enemies = _enemies;

        nextTurn();
        BattleStarted(this, EventArgs.Empty);
    }
    private void nextTurn()
    {
        
        activeCharacter = calculateNextCharactersTurn();
        if (activeCharacter != null)
        {
            activeCharacter.currentSpeed = 0;
        }
        if (activeCharacter.isAlive)
        {
            if (activeCharacter.typeOfBattler == "player")
            {
                refreshIndicatorBars();
            }
        }
    }

    //by the time increases the speed when a character reaches the cap its their turn
    private Battle_Capability_Handler calculateNextCharactersTurn()
    {
        Battle_Capability_Handler temp=null;

        while (temp == null)
        {
            
            temp = checkIfSomebodyHasFull();
            if (temp != null)
            {
                return temp;
            }
            foreach (var item in friendlies)
            {
                if (item.isAlive)
                {
                    item.currentSpeed += item.baseSpeed + item.speedModifier;
                }
            }
            foreach (var item in enemies)
            {
                if (item.isAlive)
                {
                    item.currentSpeed += item.baseSpeed + item.speedModifier;
                }
            }
        }
        return null;
    }

    private Battle_Capability_Handler checkIfSomebodyHasFull()
    {
        var retVal = (Battle_Capability_Handler) friendlies.Find(x => x.currentSpeed >= 100);
        if (retVal != null)
        {
            return retVal;
        }
        retVal = (Battle_Capability_Handler)enemies.Find(x => x.currentSpeed >= 100);
        if (retVal !=null)
        {
            return retVal;
        }
        else
        {
            return null;
        }
    }


    public void useSkill()
    {

    }

    public void Button2_Clicked()
    {
        RecieveDamage_ToCurrentlyActivePartyMember(3);
        turnEnding();
    }

    //returns If player died to the dmg recieved
    public bool RecieveDamage_ToCurrentlyActivePartyMember(int amount)
    {
        activeCharacter.CurrentHealth = activeCharacter.CurrentHealth - amount;
        if (activeCharacter.CurrentHealth <= 0)
        {
            activeCharacter.isAlive = false;
            return false;
        }
        refreshIndicatorBars();
        return true;
    }

    private void refreshIndicatorBars()
    {
        HpDisplayingText.text = activeCharacter.CurrentHealth.ToString();
    }

    private void turnEnding()
    {
        refreshIndicatorBars();
        countBackForNextTurnStart = 3.0f;
    }

    //temporary end fight event
    public void battleUIButtonClicked()
    {
        BattleUI.enabled = false;
        BattleFinished(this, EventArgs.Empty);
    }
}
