using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Intro : MonoBehaviour
{
    public float FadeDuration = 1f;
    public Color Color1 = Color.gray;
    public Color Color2 = Color.white;

    private Color startColor;
    private Color endColor;
    private float lastColorChangeTime;

    public Text text;

    void Start()
    {
        startColor = Color1;
        endColor = Color2;
    }

    void Update()
    {
        var ratio = (Time.time - lastColorChangeTime) / FadeDuration;
        ratio = Mathf.Clamp01(ratio);

        if(text != null) text.color = Color.Lerp(startColor, endColor, ratio);

        if (ratio == 1f)
        {
            lastColorChangeTime = Time.time;

            // Switch colors
            var temp = startColor;
            startColor = endColor;
            endColor = temp;
        }

        if(Input.GetButtonDown("Pulo"))
        {
            text.gameObject.SetActive(false);
            LeanTween.alpha(this.gameObject.GetComponent<Image>().rectTransform, 0f, 4f).setEase(LeanTweenType.linear);
            HUDManager.hUDManager.termoObject.SetActive(true);
        }

        if(this.gameObject.GetComponent<Image>().color.a == 0f)
        {
            Destroy(this.gameObject);
        }
    }
}
