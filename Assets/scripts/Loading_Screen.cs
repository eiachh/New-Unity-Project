using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class Loading_Screen : MonoBehaviour
{
    float counter = 0;
    public Text LoadingText;
    bool alreadyEntered = false;

    GameObject character;
    WorldPositionHolder worldPos;

    public string SceneToLoad = "FirstScene";

    public Canvas UI_LoadingScreen;

    public delegate void BeforeSceneChangeEventHandler(object sender, EventArgs e);
    public event BeforeSceneChangeEventHandler BeforeSceneChange;

    public float timeToWait = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;

        

        var temp = FindObjectOfType<Character_Script>();
        worldPos = FindObjectOfType<WorldPositionHolder>();
        character = temp.gameObject;

        // UI_LoadingScreen.transform.position += new Vector3(0,0,-100);
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > 1 && counter < 1.5f)
        {
            LoadingText.text = "Loading Done!";
        }
        else if (counter > timeToWait && alreadyEntered == false)
        {
            alreadyEntered = true;
            UI_LoadingScreen.enabled = false;
            SceneManager.LoadScene(SceneToLoad);
        }
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        worldPos = FindObjectOfType<WorldPositionHolder>();
        Vector2 v;
        if (worldPos != null)
        {
            v = worldPos.gameObject.transform.position;
        }
        else
        {
            v = new Vector2(0, 0);
        }
        character.transform.position = v;
    }

    public void teleportTo(string _scene)
    {
        if (BeforeSceneChange!=null)
        {
            BeforeSceneChange(this, EventArgs.Empty);
        }
        SceneToLoad = _scene;
        resetUI();
        counter = 0;
    }

    private void resetUI()
    {
        alreadyEntered = false;
        LoadingText.text = "Loading..";
        UI_LoadingScreen.enabled = true;
       // UI_LoadingScreen.transform.position += new Vector3(0, 0, -10);
    }
}
