using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChartItem : MonoBehaviour
{
    [SerializeField] private RectTransform pointerPosition;
    
    public int Number { get; private set; }

    private Image _img;

    private Image Img
    {
        get
        {
            if (_img == null)
            {
                _img = GetComponent<Image>();
            }

            return _img;
        }
    }

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

        Number = number;
    }

    public void Switch(ChartItem other)
    {
        (Rect.anchoredPosition, other.Rect.anchoredPosition)
            = (other.Rect.anchoredPosition, Rect.anchoredPosition);
    }

    public void Select(bool value)
    {
        Img.color = value ? Color.green : Color.white;
    }

    public Vector2 Point()
    {
        return pointerPosition.anchoredPosition;
    }
}
