using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeIn : MonoBehaviour
{
    public Image tooltip, background;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            LeanTween.alpha(tooltip.rectTransform, 0f, .1f).setEase(LeanTweenType.linear);
            LeanTween.alpha(background.rectTransform, 0f, .1f).setEase(LeanTweenType.linear);
        }

        if(Input.GetKeyDown(KeyCode.E))
        {
            LeanTween.alpha(tooltip.rectTransform, 1f, .15f).setEase(LeanTweenType.linear);
            LeanTween.alpha(background.rectTransform, .78f, .15f).setEase(LeanTweenType.linear);
        }
    }
}
