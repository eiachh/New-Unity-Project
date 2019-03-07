using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Full_Quest : MonoBehaviour
{
    Quest_Handler QH;

    [Tooltip("The list that contains the quest parts. Parts have to be in order in the list for correct quest order.")]
    public List<Quest_PartBase> questParts = new List<Quest_PartBase>();
    public int QuestState { get; set; } = 0;

    [Tooltip("Set true if you want the quest to be available without any statement at all.")]
    public bool availableByDefault = false;
    [Tooltip("If you set this to complete the quest won't be available ever. Could be used for reference purposes.")]
    public bool completedQuest = false;
    [Tooltip("EVERY QUEST'S questID HAS TO BE UNIQUE! Make sure to set it something understandable since other quests can refer to this with \"prerequisiteQuestID\"")]
    public string questID;
    [Tooltip("Set this to the quest's questID that would unlock this quest.")]
    public string prerequisiteQuestID="none";
    [Tooltip("Set this to true if this should be AUTOMATICALLY activated after prerequisiteQuestID got completed!")]
    public bool ContinuationOfQuest = false;


    void Awake()
    {
        QH = FindObjectOfType<Quest_Handler>();
        QH.OnFullQuestRegisterRequired += register;
    }
    public void register(object sender, EventArgs e)
    {
        QH.addToQuestList(this, availableByDefault,completedQuest);
    }

    public Npc_Script getTargetNpc()
    {
        return questParts[QuestState].Questtarget;
    }

    public bool getVisibleMark()
    {
        return questParts[QuestState].visibleMark;
    }

    public string getNpcText()
    {
        return questParts[QuestState].NpcQuestText;
    }
    public void taskCompleted()
    {
        //the bool is never used YET in references
        if (QuestState == questParts.Count-1)
        {
            completedQuest = true;
            QH.questCompleted(questID,questParts[QuestState].CanBeQuestEnd);
        }
        else
        {
            QuestState++;
            
            questParts[QuestState].PrepareQuestPart(questID,false);
        }
    }

    //when SceneLoaded Happened and has to get the previous lost state
    public void OnLoadSetup(int _state)
    {
        QuestState = _state;
        if (QuestState == questParts.Count)
        {
            completedQuest = true;
            QH.questCompleted(questID,questParts[QuestState].CanBeQuestEnd);
        }
        else
        {
            questParts[QuestState].PrepareQuestPart(questID,true);
        }
        
    }

    public void ReverseStateTo(int value)
    {
        QuestState=value;
        questParts[QuestState].PrepareQuestPart(questID, false);
    }
}
