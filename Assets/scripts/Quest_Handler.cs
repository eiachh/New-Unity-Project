using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;


public class Quest_Handler : MonoBehaviour
{

    List<Full_Quest> allQuestList = new List<Full_Quest>();


    //Tuple contains questID, QuestState  NEEDED BECAUSE ON SCENE LOAD THE REFERENCE IS BEING DELETED BUT CANT LET THEM NOT BE DELETED
    List<Tuple<string,int>> activeQuestList = new List<Tuple<string,int>>();
    //Tuple contains questID, QuestState  NEEDED BECAUSE ON SCENE LOAD THE REFERENCE IS BEING DELETED BUT CANT LET THEM NOT BE DELETED
    List<Tuple<string,int>> availableQuestList = new List<Tuple<string,int>>();

    //contains the questID's that are completed
    List<string> completedQuestList = new List<string>();

    public delegate void QuestListInitializeFinishedEventHandler(object sender, EventArgs args);
    public event QuestListInitializeFinishedEventHandler QuestListInitializeFinished;

    public CanvasGroup QuestCompletedUI;
    public Canvas QuestCompletedCanvas;

    UI_Fade fader;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
        Battle_Handler Battle_H = FindObjectOfType<Battle_Handler>();
        fader = FindObjectOfType<UI_Fade>();
        Battle_H.BattleFinished += OnBattlefinished;
        //QuestListInitializeFinished(EventArgs.Empty);
        //Calling the event so the quests can initialize themselves
        if (QuestListInitializeFinished != null)
        {
            QuestListInitializeFinished(this, EventArgs.Empty);
            handleAvailableQuests();
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cleanUpLists();
        if (QuestListInitializeFinished != null)
        {
            
            QuestListInitializeFinished(this, EventArgs.Empty);
            handleAvailableQuests();
        }
    }

    void OnBattlefinished(object sender,Battle_Handler.BattleFinishedEventArgs e)
    {
        Debug.Log("yep bbattle has finished and the winner is: "+e.WinnerTeam);
    }

    private void cleanUpLists()
    {
        allQuestList.RemoveAll(x => x==null);
        availableQuestList.Clear();
        /* for (int i = 0; i < allQuestList.Count; i++)
         {
             if (allQuestList[i] == null)
             {
                 allQuestList.RemoveAt(i);
                 i--;
             }
         }

         foreach (var item in allQuestList)
         {
             if (item==null)
             {
                 allQuestList.Remove(item);
             }
         }*/
    }

    //after initializing the lists the QuestListInitializeFinished event fires and tells all the single_quests to register themselfes this lets you create quests from editor
    public void addToQuestList(Full_Quest x, bool defaultAvailable, bool completed)
    {
        //THIS CHECKS NEEDED FOR THE OnLoadScene() event so the quest wont register themselves multiple times


        /*if allquestlist not contains this quest => the quest is new and has to be handled
                      if its available by default =>has to be added to availablequestList no other statement needed
       if the prerequisite is none and not default=>its a quest that will be locally activated by another quest
       if the prerequisite is NOT none and the prerequisit quest is completed before ===> it is valid to be an Available quest */

        if (x != null)
        {
            //Debug.Log("!=null");
            if (!allQuestList.Contains(x))
            {
               // Debug.Log("!allQuest.Contains");
                allQuestList.Add(x);
                if (!completed)
                {
                   // Debug.Log("!completed");
                    //can be already accepted if we jsut come back to the scene where this quest is placed 
                    if (defaultAvailable)
                    {
                       // Debug.Log("default");
                        var findID = activeQuestList.Find(y => y.Item1 == x.questID);
                        //IF NOT CONTAINING THIS QUEST
                        if (findID!=null)
                        {
                            allQuestList.Find(y => y.questID == x.questID).OnLoadSetup(findID.Item2);
                        }
                        else if (!completedQuestList.Contains(x.questID))
                        {
                            availableQuestList.Add(new Tuple<string, int>(x.questID, x.QuestState));
                        }
                    }
                    else if (x.prerequisiteQuestID != "none" && (completedQuestList.Find(y => y == x.prerequisiteQuestID) != null))
                    {
                        var findID = activeQuestList.Find(y => y.Item1 == x.questID);
                        //this point is reached if its not a quest starting point BUT the player has a quest that he progressed BUT didnt finish yet
                        if (findID != null)
                        {
                            allQuestList.Find(y => y.questID == x.questID).OnLoadSetup(findID.Item2);
                        }
                        else if (!completedQuestList.Contains(x.questID))
                        {                            
                            availableQuestList.Add(new Tuple<string, int>(x.questID, x.QuestState));
                        }
                    }
                }
                //probably never will enter because quests register at screen load when the default values are loaded THEREFORE
                //   PLACEHOLDER FOR LATER WHEN SAVEGAME WILL BE A THING
                else
                {
                    if (!completedQuestList.Contains(x.questID))
                    {
                        completedQuestList.Add(x.questID);
                    }
                }
            }





           /* if (x.prerequisiteQuestID == "none")
            {
                if (defaultAvailable == true && completed == false)
                {
                    availableQuestList.Add(x);
                }
            }
            else
            {
                var completedListContains = completedQuestList.Find(y => y.prerequisiteQuestID == x.prerequisiteQuestID);
                if (completedListContains != null)
                {
                    allQuestList.Add(x);
                    if (defaultAvailable == true && completed == false)
                    {
                        availableQuestList.Add(x);
                    }
                }
            }*/
        }
    }

    


    //Need fix if its not a talking quest this will throw an error (even tho before battle you probably want to talk) Questionable
    private void handleAvailableQuests()
    {
        foreach (var item in availableQuestList)
        {
           // Debug.Log(item);
            foreach (var item2 in allQuestList)
            {
               // Debug.Log("Item2: "+item2);
            }
            var getQuestReference = allQuestList.Find(x => x.questID == item.Item1);
            if (getQuestReference != null)
            {
                getQuestReference.getTargetNpc().addAvailableQuest(getQuestReference.questID, getQuestReference.getVisibleMark());
            }
            else
            {
                Debug.Log("Serious ERROR availableQuestList contains id that is not exceist in allquestList");
            }
        }
    }

    public string getNpcQuestStartingText(string ID)
    {
        var temp = allQuestList.Find(x => x.questID == ID);
        if (temp != null)
        {
            return temp.getNpcText();
        }
        return "";
    }
    //idk why this exceists but it does something? i guess
    public string getNpcQuestEndText(string ID)
    {
        var temp = allQuestList.Find(x => x.questID == ID);
        if (temp != null)
        {
            return temp.getNpcText();
        }
        return "";
    }

    public void questAcceptedWithID(string ID)
    {
        var temp = availableQuestList.Find(x => x.Item1 == ID);
        var questRef = allQuestList.Find(x => x.questID == ID);
        if (temp != null)
        {
            questRef.taskCompleted();
            //state increased since quest got accepted
            Tuple<string, int> questID_state = new Tuple<string, int>(temp.Item1,temp.Item2+1);
            

            activeQuestList.Add(questID_state);
            availableQuestList.Remove(temp);

        }
    }
    public void taskCompletedWithID(string ID)
    {
        var temp = allQuestList.Find(x => x.questID == ID);

        //it looks this retarded since tUpLe Is ReAd oNly I can be bothered fix it if you see it and can be bothered
        //#Fix
        var toIncrease = activeQuestList.Find(x => x.Item1 == ID);
        
        Tuple<string, int> questID_state = new Tuple<string, int>(toIncrease.Item1, toIncrease.Item2 + 1);
        activeQuestList.Remove(toIncrease);
        activeQuestList.Add(questID_state);
        if (temp != null)
        {
            //temp.getTargetNpc().addActiveQuest(temp.questID, temp.getVisibleMark());
            temp.taskCompleted();
        }
    }

    public void questCompleted(string qID)
    {

        var temp = activeQuestList.Find(x => x.Item1 == qID);
        activeQuestList.Remove(temp);

        completedQuestList.Add(qID);


        Debug.Log("questcomp UI: " + QuestCompletedUI);
        QuestCompletedCanvas.enabled = true;
        QuestCompletedUI.enabled = true;
        QuestCompletedUI.alpha = 1.0f;

        fader.FadeOut(QuestCompletedUI);
    }

    public void ReverseStateWithOne(string qID)
    {
        var temp = activeQuestList.Find(x => x.Item1 == qID);
        if (temp.Item2 == 1)
        {
            activeQuestList.Remove(temp);
            availableQuestList.Add(temp);
        }
        else if (temp.Item2==0)
        {
            Debug.Log("Error activeQuestlist at quID: " + temp.Item1 + " state is 0 and is reversing with one");
        }
        else
        {
            Tuple<string, int> questID_state = new Tuple<string, int>(temp.Item1, (temp.Item2 - 1));
            activeQuestList.Remove(temp);
            activeQuestList.Add(questID_state);
            var fullquest = allQuestList.Find(x => x.questID == qID);
            fullquest.ReverseStateTo(questID_state.Item2);
        }
    }

    /*public void registerQuestStateUpdate()
    {

    }*/

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
