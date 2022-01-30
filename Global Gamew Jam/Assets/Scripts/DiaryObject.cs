using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiaryObject : MonoBehaviour
{
    public Image tooltip, background;
    private bool canInteract;
    private Transform player;

    // Start is called before the first frame update
    void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(transform.position, player.position)<1.95f)
        {
            canInteract = true;
            if(tooltip.color.a < 1)
            {
                LeanTween.alpha(tooltip.rectTransform, 1f, .15f).setEase(LeanTweenType.linear);
                LeanTween.alpha(background.rectTransform, .78f, .18f).setEase(LeanTweenType.linear);
            }

        } else
        {
            canInteract = false;
            if(tooltip.color.a > 0)
            {
                LeanTween.alpha(tooltip.rectTransform, 0f, .1f).setEase(LeanTweenType.linear);
                LeanTween.alpha(background.rectTransform, 0f, .12f).setEase(LeanTweenType.linear);
            }
        }

        if(Input.GetButtonDown("Pulo") && canInteract)
        {
            HUDManager.hUDManager.openDiary();
            canInteract = false;
        }
    }
}
