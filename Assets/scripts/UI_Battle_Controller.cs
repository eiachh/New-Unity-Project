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

    string temp="G";

    void Start()
    {
        menuButtomHeight = menuCustomButtons.GetComponent<RectTransform>().sizeDelta.y;
    }
    public void addbuttonto_listview()
    {
        temp += "G";
        var asd = Instantiate(menuCustomButtons);
        asd.GetComponentInChildren<Text>().fontStyle = FontStyle.Bold;
        asd.GetComponentInChildren<Text>().text = (temp);

       asd.transform.parent = mainContainerParent.transform;
    }
    public void moveButtonsUp()
    {
        //transform position is not relative like this even tho the editor says 0,0 compared to parent
        //the 0,0 of screen is the bottom left corner even when you change the anchor
        float getButton_y = mainContainerParent.transform.position.y - mainContainerParent.transform.GetComponent<RectTransform>().sizeDelta.y;
        float getMask_y = MaskForLocations.transform.position.y - MaskForLocations.transform.GetComponent<RectTransform>().sizeDelta.y;
       // float getSelecter_y = Selecter.transform.position.y - Selecter.transform.GetComponent<RectTransform>().sizeDelta.y;
      // Debug.Log(getSelecter_y +"selecter container: "+ mainContainerParent.transform.position.y);
        if (getButton_y < getMask_y)
        {
            mainContainerParent.transform.position = new Vector2(mainContainerParent.transform.position.x, mainContainerParent.transform.position.y + menuButtomHeight);
        }
        
    }
    public void moveButtonsDown()
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
}
