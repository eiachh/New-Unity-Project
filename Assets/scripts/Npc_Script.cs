using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Npc_Script : Interractable
{
    GameObject questQuestionMarkPic;
    GameObject questExclamation;

    Quest_Handler QH;

    public List<Tuple<string,bool>> availableQuestIDs = new List<Tuple<string, bool>>();
    public List<Tuple<string, bool>> activeQuestIDs = new List<Tuple<string, bool>>();

    private GameObject questAvailableToShow;

    bool availableMarked = false;
    bool activeMarked = false;

    bool delayRefresh = true;

    void Start()
    {
        QH = FindObjectOfType<Quest_Handler>();

        questQuestionMarkPic = (GameObject)Resources.Load("prefabs/Quest_Questionmark", typeof(GameObject));
        questExclamation = (GameObject)Resources.Load("prefabs/Quest_Exclamation", typeof(GameObject));

        var loading_screen = FindObjectOfType<Loading_Screen>();
        loading_screen.BeforeSceneChange += destroyUI;

        delayRefresh = false;
        refreshMarks();
    }

    private void destroyUI(object sender,EventArgs e)
    {
        //fix this
    }

    public override bool Interract() 
    {
        if (activeQuestIDs.Count>0)
        {
            string textToPrint = QH.getNpcQuestEndText(activeQuestIDs[0].Item1);
            return base.startInterraction(textToPrint);
        }
        else if (availableQuestIDs.Count>0)
        {
            string textToPrint = QH.getNpcQuestStartingText(availableQuestIDs[0].Item1);
            return base.startInterraction(textToPrint);
        }
        else
        {
            return base.startInterraction("");
        }
    }

    public override bool nextDialog()
    {
        return base.nextDialog(this);
    }

    public void addAvailableQuest(string ID, bool visibleMark)
    {
        availableQuestIDs.Add(new Tuple<string, bool>(ID, visibleMark));
        if (visibleMark)
        {
            availableMarked = true;
            refreshMarks();
        }
    }
    public void addActiveQuest(string ID, bool visibleMark)
    {
        activeQuestIDs.Add(new Tuple<string, bool>(ID, visibleMark));
        if (visibleMark)
        {
            availableMarked = true;
            refreshMarks();
        }
    }

    public void refreshMarks()
    {
        if (!delayRefresh)
        {
            
            if (activeQuestIDs.Count > 0)
            {
                if (activeQuestIDs[0].Item2)
                {
                    if (questAvailableToShow != null)
                    {
                        Destroy(questAvailableToShow);
                    }
                    showExclamationMark();
                }
            }     
            else if (availableQuestIDs.Count > 0)
            {
                if (availableQuestIDs[0].Item2)
                {
                    if (questAvailableToShow !=null)
                    {
                        Destroy(questAvailableToShow);
                    }
                    showQuestionMark();
                }
            }
            else if (availableQuestIDs.Count==0 && activeQuestIDs.Count==0)
            {
                if (questAvailableToShow != null)
                {
                    Destroy(questAvailableToShow);
                }
            }
        }
    }

    //deletes the top (0) list element because the quest had been taken/finished normally
    public void textReadingNormallyFinished()
    {
        if (activeQuestIDs.Count > 0)
        {
            taskCompleted();
            
            activeQuestIDs.RemoveAt(0);
            refreshMarks();
            
        }
        else if (availableQuestIDs.Count > 0)
        {
            questAccepted();
            
            availableQuestIDs.RemoveAt(0);
            refreshMarks();
           
        }
    }

    public void questAccepted()
    {
        QH.questAcceptedWithID(availableQuestIDs[0].Item1);
    }

    public void taskCompleted()
    {  
        QH.taskCompletedWithID(activeQuestIDs[0].Item1);
    }

    public void showQuestionMark()
    {
        questAvailableToShow= Instantiate(questQuestionMarkPic);
        questAvailableToShow.GetComponent<RectTransform>().localPosition = new Vector3(this.transform.position.x, this.transform.position.y+2, 10);
    }
    public void showExclamationMark()
    {
        questAvailableToShow = Instantiate(questExclamation);
        questAvailableToShow.GetComponent<RectTransform>().localPosition = new Vector3(this.transform.position.x, this.transform.position.y + 2, 10);
    }
}
