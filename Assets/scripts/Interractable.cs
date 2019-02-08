using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interractable : MonoBehaviour
{
    public GameObject self;
    public GameObject hitbox;
    private Text text;


    GameObject canvasGO;

    public string TextToPrint;
    private List<string> listToPrint = new List<string>();
    int currentIndexOfList = 0;

    bool alreadyInterracted = false;
    public void divide(string s)
    {
        listToPrint.Clear();
        listToPrint.AddRange(s.Split('§'));
    }

    //always false so you cant interract with something thats just interractable this class is meant to be the parent class with virtual functions
    public virtual bool Interract()
    {
        return false;
    }
    protected bool startInterraction(string customizeableText)
    {

            canvasGO = new GameObject();

            // Load the Arial font from the Unity Resources folder.
            Font arial;
            arial = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

            // Create Canvas GameObject.

            canvasGO.name = "Canvas";
            canvasGO.AddComponent<Canvas>();
            canvasGO.AddComponent<CanvasScaler>();
            canvasGO.AddComponent<GraphicRaycaster>();
            canvasGO.GetComponent<RectTransform>().localScale = new Vector3(0.005f, 0.005f, 0.0f);
            canvasGO.GetComponent<RectTransform>().sizeDelta = new Vector3(900.0f, 100.0f, 3.0f);

            //float tempx = hitbox.GetComponent<RectTransform>().localPosition.x;
            float tempx = self.transform.position.x;
            float tempy = self.transform.position.y + 2;
            canvasGO.GetComponent<RectTransform>().localPosition = new Vector3(tempx, tempy, 3.0f);

            // Get canvas from the GameObject.
            Canvas canvas;
            canvas = canvasGO.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.WorldSpace;

            //image to have a background
            GameObject imageGO = new GameObject(); //Create the GameObject
            Image NewImage = imageGO.AddComponent<Image>(); //Add the Image Component script
            NewImage.sprite = null;
            NewImage.color = Color.black;
            //NewImage.sprite = currentSprite; //Set the Sprite of the Image Component on the new GameObject
            imageGO.GetComponent<RectTransform>().SetParent(canvasGO.transform); //Assign the newly created Image GameObject as a Child of the Parent Panel.
            imageGO.SetActive(true); //Activate the GameObject
            imageGO.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 0.0f);
            imageGO.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, 0.0f, 3.0f);

            float temp2x = canvas.GetComponent<RectTransform>().sizeDelta.x;
            float temp2y = canvas.GetComponent<RectTransform>().sizeDelta.y;

            imageGO.GetComponent<RectTransform>().sizeDelta = new Vector3(temp2x, temp2y, 3.0f);


            //imageGO.transform.parent = canvasGO.transform;
            //Image backPick = new Image();


            // Create the Text GameObject.
            GameObject textGO = new GameObject();
            textGO.transform.parent = canvasGO.transform;
            textGO.AddComponent<Text>();

            // Set Text component properties.
            text = textGO.GetComponent<Text>();
            text.font = arial;

            ///Print row by row
            ///
            if (customizeableText == null || customizeableText == "")
            {
                divide(TextToPrint);
            }
            else
            {
                
                divide(customizeableText);
            }
        ///Print row by row
        alreadyInterracted = true;
        text.text = listToPrint[currentIndexOfList];
        currentIndexOfList++;

        text.fontSize = 48;
            text.alignment = TextAnchor.MiddleCenter;

            text.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 0.0f);
            text.GetComponent<RectTransform>().localPosition = new Vector3(0.0f, 0.0f, 5.0f);


            // Provide Text position and size using RectTransform.
            RectTransform rectTransform;
            rectTransform = text.GetComponent<RectTransform>();
            //rectTransform.localPosition = new Vector3(0, 0, 0);
            rectTransform.sizeDelta = new Vector2(temp2x, temp2y);

            return true;
    }
    protected virtual bool nextDialog(Npc_Script caller)
    {
        if (currentIndexOfList < listToPrint.Count && listToPrint[0] != "")
        {
            alreadyInterracted = true;
            text.text = listToPrint[currentIndexOfList];
            currentIndexOfList++;
            return true;
        }
        else
        {
            caller.textReadingNormallyFinished();
            currentIndexOfList = 0;
            alreadyInterracted = false;
            deleteMessage();
            return false;
        }
    }

    private void deleteMessage()
    {
        currentIndexOfList = 0;
        alreadyInterracted = false;
        Destroy(canvasGO);
    }

    public void interruptMessage()
    {
        currentIndexOfList = 0;
        alreadyInterracted = false;
        Destroy(canvasGO);
    }

    //fake not useable from this class NPC_script overwrites it
    public virtual bool nextDialog()
    {
        return false;
    }
}
