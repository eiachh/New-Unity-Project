using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


public class Quest_Handler : MonoBehaviour
{

    List<Full_Quest> allQuestList = new List<Full_Quest>();
    List<Full_Quest> activeQuestList = new List<Full_Quest>();
    List<Full_Quest> availableQuestList = new List<Full_Quest>();

    public delegate void QuestListInitializeFinishedEventHandler(object sender, EventArgs args);
    public event QuestListInitializeFinishedEventHandler QuestListInitializeFinished;

    // Start is called before the first frame update
    void Start()
    {
        
        //QuestListInitializeFinished(EventArgs.Empty);
        //Calling the event so the quests can initialize themselves
        QuestListInitializeFinished(this, EventArgs.Empty);
        handleAvailableQuests();
    }

    //after initializing the lists the QuestListInitializeFinished event fires and tells all the single_quests to register themselfes this lets you create quests from editor
    public void addToQuestList(Full_Quest x, bool defaultAvailable,bool completed)
    {
        allQuestList.Add(x);
        if (defaultAvailable==true && completed==false)
        {
            availableQuestList.Add(x);
        }
    }



    private void handleAvailableQuests()
    {
        foreach (var item in availableQuestList)
        {
            item.getTargetNpc().addAvailableQuest(item.questID, item.getVisibleMark());

        }
    }

    public string getNpcQuestStartingText(string ID)
    {
        var temp = availableQuestList.Find(x => x.questID == ID);
        if (temp != null)
        {
            return temp.getNpcText();
        }
        return "";
    }
    //idk why this exceists but it does something? i guess
    public string getNpcQuestEndText(string ID)
    {
        var temp = activeQuestList.Find(x => x.questID == ID);
        if (temp != null)
        {
            return temp.getNpcText();
        }
        return "";
    }

    public void questAcceptedWithID(string ID)
    {
        var temp = availableQuestList.Find(x => x.questID == ID);
        if (temp != null)
        {
            // temp.getTargetNpc().addActiveQuest(temp.questID, temp.getVisibleMark());
            temp.taskCompleted();
            activeQuestList.Add(temp);
            availableQuestList.Remove(temp);
        }
    }
    public void taskCompletedWithID(string ID)
    {
        var temp = activeQuestList.Find(x => x.questID == ID);
      
         if (temp != null)
        {
            //temp.getTargetNpc().addActiveQuest(temp.questID, temp.getVisibleMark());
            temp.taskCompleted();
        }
    }

    /*public bool doesQuestExceist(string ID,bool isActive)
    {
        var temp = activeQuestList.Find(x => x.questid == ID);
        if (temp==null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    */
}
