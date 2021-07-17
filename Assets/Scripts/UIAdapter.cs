using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAdapter : MonoBehaviour
{
    void Start()
    {
        float k = Mathf.Max(Screen.width/1280f, Screen.height/720f);

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta *= k;
        rectTransform.anchoredPosition *= k;
    }
}
