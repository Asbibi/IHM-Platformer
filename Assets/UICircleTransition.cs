using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICircleTransition : MonoBehaviour
{
    [SerializeField] Animator maskAnimator = null;
    [SerializeField] Animator circleAnimator = null;
    bool inAnimation = false;
    bool inMiddleOfTransition = false;
    public float startDuration = 0.7f;
    public float endDuration = 0.7f;

    IEnumerator StartTransition()
    {
        if (!inMiddleOfTransition && !inAnimation)
        {
            inAnimation = true;
            maskAnimator.gameObject.SetActive(true);
            maskAnimator.enabled = false;
            circleAnimator.enabled = true;
            yield return new WaitForSeconds(startDuration);

            inAnimation = false;
            inMiddleOfTransition = true;
        }
    }
    IEnumerator EndTransition()
    {
        if (inMiddleOfTransition && !inAnimation)
        {
            inAnimation = true;

            maskAnimator.enabled = true;
            yield return new WaitForSeconds(endDuration);
            maskAnimator.enabled = false;
            maskAnimator.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            maskAnimator.gameObject.SetActive(false);

            inAnimation = false;
            inMiddleOfTransition = false;
        }
    }


    public void ForceInMiddleOfTransition()
    {
        StopAllCoroutines();
        
        maskAnimator.gameObject.SetActive(true);
        maskAnimator.enabled = false;
        maskAnimator.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        circleAnimator.enabled = false;
        circleAnimator.GetComponent<RectTransform>().sizeDelta = new Vector2(1024,1024);

        inAnimation = false;
        inMiddleOfTransition = true;
    }

    public void PlayTransition()
    {
        StartCoroutine(StartTransition());
        StartCoroutine(EndTransition());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(StartTransition());
            StartCoroutine(EndTransition());
        }
        if (Input.GetKeyDown(KeyCode.L))
            ForceInMiddleOfTransition();
    }

}
