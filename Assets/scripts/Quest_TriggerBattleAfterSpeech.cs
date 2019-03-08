using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Quest_TriggerBattleAfterSpeech : Quest_PartBase
{
    Battle_Handler BattleH;
    Quest_Handler QH;
    List<User_Battle_Unit> friendlyParty= new List<User_Battle_Unit>();
    public List<User_Battle_Unit> additionalControllablePartyMembers;
    public List<Enemy_Base> enemiesToFight;

    Canvas Battle_UI;

    string questID;
    void Awake()
    {

        BattleH= FindObjectOfType<Battle_Handler>();
        QH = FindObjectOfType<Quest_Handler>();

        var Battle_Cont = FindObjectOfType<UI_Battle_Controller>();
       Battle_UI = Battle_Cont.gameObject.GetComponent<Canvas>();

        

        var temp = FindObjectOfType<Unique_Party>();
        friendlyParty.AddRange(temp.ControllablePartyMembers);
        friendlyParty.AddRange(additionalControllablePartyMembers);

        //BattleH.BattleFinished += Battle_Has_Finished;
    }

    public override void PrepareQuestPart(string _questID,bool OnLoadTriggered)
    {
        
        questID = _questID;
        if (!OnLoadTriggered)
        {
            Debug.Log("Activating REEEEEEE");
            Battle_UI.enabled = true;
            BattleH.initiateBattleAtPremadeArena(friendlyParty, enemiesToFight, questID, this.gameObject.scene.name);
        }
        else
        {
            QH.ReverseStateWithOne(questID);
        }
        
    }

    public void Battle_Has_Finished(object sender, EventArgs e)
    {

        //QH.taskCompletedWithID(questID);
    }
}
