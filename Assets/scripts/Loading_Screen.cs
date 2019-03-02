using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Loading_Screen : MonoBehaviour
{
    float counter = 0;
    public Text LoadingText;
    bool alreadyEntered = false;

    public Canvas UI_LoadingScreen;

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
        else if (counter>1.5f && alreadyEntered==false)
        {
            alreadyEntered = true;
            UI_LoadingScreen.enabled = false;
            SceneManager.LoadScene("FirstScene");

        }
    }
}
