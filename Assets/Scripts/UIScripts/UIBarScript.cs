using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBarScript : MonoBehaviour
{
    public Image mask;
    Vector2 originalSize;
    void Start()
    {
        
        originalSize.x = mask.rectTransform.rect.width;
        originalSize.y = mask.rectTransform.rect.height;
    }

    public void SetValue(bool horizontal, float value)
    {
        if (horizontal)
        {
            mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, originalSize.x * value);
        }
        else
        {
            Debug.Log(value);
            mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize.y * value);
        }
    }
}
