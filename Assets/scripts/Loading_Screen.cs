using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Loading_Screen : MonoBehaviour
{
    float counter = 0;
    public Text LoadingText;
    bool alreadyEntered = false;


    public string SceneToLoad = "FirstScene";

    public Canvas UI_LoadingScreen;

    public delegate void BeforeSceneChangeEventHandler(object sender, EventArgs e);
    public event BeforeSceneChangeEventHandler BeforeSceneChange;

    public float timeToWait = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        if (counter>1 && counter<1.5f)
        {
            LoadingText.text = "Loading Done!";
           // SceneManager.LoadScene("FirstScene");
        }
        else if (counter>timeToWait && alreadyEntered==false)
        {
            alreadyEntered = true;
            UI_LoadingScreen.enabled = false;
            SceneManager.LoadScene(SceneToLoad);

        }
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
    }
}
