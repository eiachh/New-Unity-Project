using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Battle_Handler : MonoBehaviour
{
    public delegate void BattleStartedEventHandler(object sender, EventArgs e);
    public event BattleStartedEventHandler BattleStarted;

    public delegate void BattleFinishedEventHandler(object sender, EventArgs e);
    public event BattleFinishedEventHandler BattleFinished;

    public Canvas BattleUI;

   public void initiateBattle()
    {
        BattleUI.enabled = true;
        BattleStarted(this, EventArgs.Empty);
    }

    public void battleUIButtonClicked()
    {
        BattleUI.enabled = false;
        BattleFinished(this, EventArgs.Empty);
    }
}
