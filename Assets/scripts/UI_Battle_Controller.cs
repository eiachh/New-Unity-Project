using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Battle_Controller : MonoBehaviour
{
    public Canvas Battle_UI;
    public Image skillListView;
    public Image MaskForLocations;
    public Image Selecter;
    public Image playArea;

    public Image mainContainerParent;
    public Button menuCustomButton;
    float menuButtomHeight;

    List<Button> menuButtonList = new List<Button>();
    int verticalIndexerOfmenuButtonList = 0;

    public Text HealthText;

    Battle_Handler Battle_H;

    List<Enemy_Base> enemiesForVisual = new List<Enemy_Base>();
    List<User_Battle_Unit> friendliesForVisual = new List<User_Battle_Unit>();

    int characterPlacementAlreadyTakenSpace = 0;


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        menuButtomHeight = menuCustomButton.GetComponent<RectTransform>().sizeDelta.y;
        Battle_H = FindObjectOfType<Battle_Handler>();
    }

    public void UpdateUIWithBattler(Battle_Capability_Handler battler)
    {
        Battle_UI.enabled = true;
        menuButtonList.Clear();
        foreach (var skill in battler.SkillList)
        {
            addButtonTo_Listview(skill);

        }
    }
    public void UpdateUIBars(int _currentHealth)
    {
        HealthText.text = _currentHealth.ToString();
    }
    private void addButtonTo_Listview(Skill_Base skill)
    {
        //this is the first button thats already in the container
        if (menuButtonList.Count == 0)
        {
            menuCustomButton.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
            menuCustomButton.GetComponentInChildren<Text>().text = (skill.name);

            menuCustomButton.gameObject.AddComponent<Button_SkillRef>();
            menuCustomButton.gameObject.GetComponent<Button_SkillRef>().SkillAttached = skill;

            menuCustomButton.onClick.AddListener(
                delegate 
                {
                    useSelectedSkill(menuCustomButton.gameObject.GetComponent<Button_SkillRef>().SkillAttached);
                });
            menuButtonList.Add(menuCustomButton);
        }
        else
        {
            var instantiatedButton = Instantiate(menuCustomButton);
            instantiatedButton.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
            instantiatedButton.GetComponentInChildren<Text>().text = (skill.name);

            instantiatedButton.gameObject.AddComponent<Button_SkillRef>();
            instantiatedButton.gameObject.GetComponent<Button_SkillRef>().SkillAttached = skill;

            instantiatedButton.transform.parent = mainContainerParent.transform;
            instantiatedButton.onClick.AddListener(
                delegate 
                {
                    useSelectedSkill(instantiatedButton.gameObject.GetComponent<Button_SkillRef>().SkillAttached);
                });
            menuButtonList.Add(instantiatedButton);
        }
       
        
    }

    private void moveButtonsUp()
    {
        //transform position is not relative like this even tho the editor says 0,0 compared to parent
        //the 0,0 of screen is the bottom left corner even when you change the anchor
        float getButton_y = mainContainerParent.transform.position.y - mainContainerParent.transform.GetComponent<RectTransform>().sizeDelta.y;
        float getMask_y = MaskForLocations.transform.position.y - MaskForLocations.transform.GetComponent<RectTransform>().sizeDelta.y;
        if (getButton_y < getMask_y)
        {
            mainContainerParent.transform.position = new Vector2(mainContainerParent.transform.position.x, mainContainerParent.transform.position.y + menuButtomHeight);
            verticalIndexerOfmenuButtonList++;
        }

    }
    private void moveButtonsDown()
    {

        if (mainContainerParent.transform.position.y > MaskForLocations.transform.position.y)
        {
            mainContainerParent.transform.position = new Vector2(mainContainerParent.transform.position.x, mainContainerParent.transform.position.y - menuButtomHeight);
            verticalIndexerOfmenuButtonList--;
        }
        //No clue why -25 the container is 0 the selecter is -25 such as the button but here the container is compared

    }

    public void selecterDown()
    {
        float getButton_y = MaskForLocations.transform.position.y - MaskForLocations.transform.GetComponent<RectTransform>().sizeDelta.y;
        float selecterBottom = Selecter.transform.position.y - Selecter.transform.GetComponent<RectTransform>().sizeDelta.y;
        float containerBottom = mainContainerParent.transform.position.y - mainContainerParent.transform.GetComponent<RectTransform>().sizeDelta.y;
        if (Selecter.transform.position.y - 25 > getButton_y && selecterBottom > containerBottom)
        {
            Selecter.transform.position = new Vector2(Selecter.transform.position.x, Selecter.transform.position.y - menuButtomHeight);
            verticalIndexerOfmenuButtonList++;
        }
        else
        {
            moveButtonsUp();
        }
    }

    public void selecterUp()
    {
        if (Selecter.transform.position.y + 25 < MaskForLocations.transform.position.y)
        {
            Selecter.transform.position = new Vector2(Selecter.transform.position.x, Selecter.transform.position.y + menuButtomHeight);
            verticalIndexerOfmenuButtonList--;
        }
        else
        {
            moveButtonsDown();
            
        }
    }

    //assigned to button Click() on instantiation at addbuttonto_listview(Skill_Base skill)
    public void useSelectedSkill(Skill_Base skillRef)
    {
        Battle_H.playerSelectedSkillToUse(skillRef);
    }

    public void selectCurrentMenuPoint()
    {
        menuButtonList[verticalIndexerOfmenuButtonList].onClick.Invoke();
    }

    public Enemy_Base selectSingleEnemy()
    {
        setupUIForEnemySelect();



        //temp #Fix
        return null;
    }

    public void setupStartCharacterLayout(List<Enemy_Base> _enemies,List<User_Battle_Unit> _friendlies,float characterWidthHeight)
    {
        enemiesForVisual.Clear();
        enemiesForVisual.AddRange(_enemies);
        friendliesForVisual.Clear();
        friendliesForVisual.AddRange(_friendlies);
        System.Random r = new System.Random();

        //iterates through the enemies and friendlies list randomly
        int enemyListIndex = 0;
        int friendlyListIndex = 0;
        //                                      -2 cause count gives back the amount not the index from 0
        while (enemyListIndex+friendlyListIndex <= (enemiesForVisual.Count+friendliesForVisual.Count) -2)
        {
            var next = r.Next(0, 2);
            //randomly decides if next placing is enemy or friendly
            if (next == 0)
            {
                
                Vector2 vectToSet = getPreferedLocationLineByLineMethod(characterWidthHeight);
                //if vect.x == -1 that means there is no more space on the battlefield so need a new method (all the rows contain at lest 1 character)
                // (one row could contain multiple character its just not set to check)
                if (vectToSet.x!=-1)
                {
                    friendliesForVisual[friendlyListIndex].transform.position = vectToSet;
                    friendlyListIndex++;
                }
                else
                {
                    //Has to finish If there is not enough space on the battlefield here
                }
            }
            //randomly decides if next placing is enemy or friendly
            else if (next ==1)
            {
                Vector2 vectToSet = getPreferedLocationLineByLineMethod(characterWidthHeight);
                //if there is enough space on the battlefield
                if (vectToSet.x != -1)
                {
                    enemiesForVisual[enemyListIndex].transform.position = vectToSet;
                    enemyListIndex++;
                }
                else
                {
                    //Has to finish If there is not enough space on the battlefield here
                }
            }
        }
    }

    //Returns -1,-1 if there is no space left
    public Vector2 getPreferedLocationLineByLineMethod(float characterWidthHeight)
    {
        //so the object not exaactly on top of each other
        characterPlacementAlreadyTakenSpace += 20;

        var randXRightSide = Mathf.RoundToInt(playArea.transform.GetComponent<RectTransform>().sizeDelta.x - characterWidthHeight);
        System.Random randomX = new System.Random();

        if (characterPlacementAlreadyTakenSpace + characterWidthHeight < playArea.transform.GetComponent<RectTransform>().sizeDelta.y)
        {
            Vector2 vect = new Vector2(randomX.Next(0, randXRightSide), characterPlacementAlreadyTakenSpace);
            characterPlacementAlreadyTakenSpace += Mathf.RoundToInt(characterWidthHeight);
            return vect;
        }
        else
        {
            return new Vector2(-1,-1);
        }
    }

    private void setupUIForEnemySelect()
    {
        skillListView.gameObject.SetActive(false);
        //skillListView.enabled = false;
    }

}
