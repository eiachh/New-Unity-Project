using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class UI_Fade : MonoBehaviour
{
    public CanvasGroup uiElement;
    public int waitTimeMillsec=0;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    public void FadeIn()
    {
        //StartCoroutine(FadeCanvasGroup(uiElement, uiElement.alpha, 1, .5f));
    }

    public void FadeOut(Canvas canvasToDisable,CanvasGroup ui, int _waitTimeMillsec = 0)
    {
        uiElement = ui;
        StartCoroutine(FadeCanvasGroup(canvasToDisable,uiElement, uiElement.alpha, 0, 2.5f,_waitTimeMillsec));
    }

    public IEnumerator FadeCanvasGroup(Canvas canvasToDisable, CanvasGroup cg, float start, float end, float lerpTime = 1,int _waitTimeMillsec =0)
    {
        float _timeStartedLerping = Time.time;
        float timeSinceStarted = Time.time - _timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;

        Thread.Sleep(_waitTimeMillsec);

        while (true)
        {
            timeSinceStarted = Time.time - _timeStartedLerping;
            percentageComplete = timeSinceStarted / lerpTime;

            float currentValue = Mathf.Lerp(start, end, percentageComplete);

            cg.alpha = currentValue;

            if (percentageComplete >= 1)
            {
                canvasToDisable.enabled = false;
                break;
            }

            yield return new WaitForFixedUpdate();
        }

    }
}
