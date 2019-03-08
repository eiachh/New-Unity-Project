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
    public Button menuCustomButtons;
    float menuButtomHeight;

    GameObject go;
    Button testButton;

    List<Button> menuButtonList = new List<Button>();

   // public List<>


    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        menuButtomHeight = menuCustomButtons.GetComponent<RectTransform>().sizeDelta.y;

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

    }
    private void addButtonTo_Listview(Skill_Base skill)
    {
        go = new GameObject();
        go.AddComponent<Button>();
        go.AddComponent<RectTransform>();
        go.AddComponent<CanvasRenderer>();
        go.AddComponent<Image>();

        testButton = go.GetComponent<Button>();

        testButton.GetComponent<RectTransform>().sizeDelta = menuCustomButtons.GetComponent<RectTransform>().sizeDelta;
        testButton.gameObject.transform.localScale = new Vector3(1, 1, 1);

        GameObject fortest = new GameObject();
        fortest.AddComponent<Text>().text="test";
        fortest.transform.parent = go.transform;

        Debug.Log("Adding button for skill: " + skill+Time.fixedTime);
        var instantiatedButton = Instantiate(menuCustomButtons);
        instantiatedButton.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
        instantiatedButton.GetComponentInChildren<Text>().text = (skill.name);
        instantiatedButton.gameObject.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        Battle_UI.enabled = false;
        Battle_UI.enabled = true;
        //instantiatedButton.tag = skill.name;

        instantiatedButton.transform.parent = mainContainerParent.transform;
        menuButtonList.Add(instantiatedButton);
        instantiatedButton.onClick.AddListener(delegate { useSelectedSkill(instantiatedButton.tag); });
    }
    public void addButtonTo_Listview()
    {
        //Debug.Log("Adding button for skill: " + skill);
        var instantiatedButton = Instantiate(go.GetComponent<Button>());
        instantiatedButton.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
        instantiatedButton.GetComponentInChildren<Text>().text = "asd";
        //instantiatedButton.tag = skill.name;

        instantiatedButton.transform.parent = mainContainerParent.transform;
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
        }
        
    }
    private void moveButtonsDown()
    {
        
        if (mainContainerParent.transform.position.y > MaskForLocations.transform.position.y)
        {
            mainContainerParent.transform.position = new Vector2(mainContainerParent.transform.position.x, mainContainerParent.transform.position.y - menuButtomHeight);
        }
        //No clue why -25 the container is 0 the selecter is -25 such as the button but here the container is compared
        
    }

    public void selecterDown()
    {
        float getButton_y = MaskForLocations.transform.position.y - MaskForLocations.transform.GetComponent<RectTransform>().sizeDelta.y;
        if (Selecter.transform.position.y - 25 > getButton_y)
        {
            Selecter.transform.position = new Vector2(Selecter.transform.position.x, Selecter.transform.position.y - menuButtomHeight);
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
}
