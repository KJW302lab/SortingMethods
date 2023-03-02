using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChartItem : MonoBehaviour
{
    private RectTransform _rect;

    public RectTransform Rect
    {
        get
        {
            if (_rect == null)
            {
                _rect = GetComponent<RectTransform>();
            }

            return _rect;
        }
    }
    
    [SerializeField] private TMP_Text txtNum;
    
    public void Initialize(float height, float width, int number, int index)
    {
        Rect.anchorMin = Rect.anchorMax = Rect.pivot = Vector2.zero;
        
        Rect.sizeDelta = new Vector2(width, height);
        Rect.anchoredPosition = new Vector2(width * index, 0);

        txtNum.text = number.ToString();
    }

    public void Switch(ChartItem other)
    {
        (Rect.anchoredPosition, other.Rect.anchoredPosition)
            = (other.Rect.anchoredPosition, Rect.anchoredPosition);
    }
}
