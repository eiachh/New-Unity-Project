using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Quest_TriggerBattleAfterSpeech : Quest_PartBase
{
    Battle_Handler BattleH;
    Quest_Handler QH;
    public List<User_Battle_Unit> friendlyParty;
    public List<Enemy_Base> enemiesToFight;

    string questID;
    void Start()
    {
        

        BattleH= FindObjectOfType<Battle_Handler>();
        QH = FindObjectOfType<Quest_Handler>();

        BattleH.BattleFinished += Battle_Has_Finished;
    }

    public override void PrepareQuestPart(string _questID)
    {
        questID = _questID;
        BattleH.initiateBattle(friendlyParty,enemiesToFight);
    }

    public void Battle_Has_Finished(object sender, EventArgs e)
    {

        QH.taskCompletedWithID(questID);
    }
}
