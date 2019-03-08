using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Battle_Controller : MonoBehaviour
{
    public Canvas Battle_UI;
    public Image MaskForLocations;
    public Image Selecter;

    public Image mainContainerParent;
    public Button menuCustomButton;
    float menuButtomHeight;

    List<Button> menuButtonList = new List<Button>();
    int verticalIndexerOfmenuButtonList = 0;

    public Text HealthText;


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        menuButtomHeight = menuCustomButton.GetComponent<RectTransform>().sizeDelta.y;

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
            menuCustomButton.onClick.AddListener(
                delegate 
                {
                    useSelectedSkill(menuCustomButton.tag);
                });
            menuButtonList.Add(menuCustomButton);
        }
        else
        {
            var instantiatedButton = Instantiate(menuCustomButton);
            instantiatedButton.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
            instantiatedButton.GetComponentInChildren<Text>().text = (skill.name);

            //instantiatedButton.tag = skill.name;

            instantiatedButton.transform.parent = mainContainerParent.transform;
            instantiatedButton.onClick.AddListener(
                delegate 
                {
                    useSelectedSkill(instantiatedButton.tag);
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
    public void useSelectedSkill(string tag)
    {
        Debug.Log("Skill name that had been used: " + tag);
    }

    public void selectCurrentMenuPoint()
    {
        menuButtonList[verticalIndexerOfmenuButtonList].onClick.Invoke();
    }
}
