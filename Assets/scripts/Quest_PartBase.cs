using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Quest_PartBase : MonoBehaviour
{
    //public Quest_Handler QH;
    //public Npc_Script startingNpc;

        //all type of quest prepares the part for itself e.g. the talking quests tells the nps they are holding a quest
    public virtual void PrepareQuestPart(string questID,bool OnLoadTriggered)
    {
        Debug.Log("This should be overwritten every time");
    }
    public Npc_Script Questtarget;
    //public string questID;

    //public bool availableByDefault = false;
    //public bool completedQuest = false;
    public bool visibleMark = true;
    //public bool startingPointOfTheQuest = false;

    public string NpcQuestText;
    public bool IsFinished = false;
    //public string NpcQuestEndText;
    // Start is called before the first frame update
    

    
}
