using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Battle_Controller : MonoBehaviour
{
    public Canvas Battle_UI;

    public Image mainContainerParent;
    public Button menuCustomButtons;
    float menuButtomHeight;

    void Start()
    {
        menuButtomHeight = menuCustomButtons.GetComponent<RectTransform>().sizeDelta.y;
    }
    public void addbuttonto_listview()
    {
        var asd = Instantiate(menuCustomButtons);

        asd.transform.parent = mainContainerParent.transform;
    }
    public void moveButtons()
    {

        mainContainerParent.transform.position = new Vector2(mainContainerParent.transform.position.x, mainContainerParent.transform.position.y + menuButtomHeight);
    }
}
