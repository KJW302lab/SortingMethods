using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChartItem : MonoBehaviour
{
    public RectTransform pointerPosition;
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

    public List<ChartItem> Switch(ChartItem other, float duration, List<ChartItem> list)
    {
        Rect.DOAnchorPos(other.Rect.anchoredPosition, duration)
            .OnStart(() => other.Rect.DOAnchorPos(Rect.anchoredPosition, duration));

        var index1 = list.IndexOf(this);
        var index2 = list.IndexOf(other);

        var temp = this;
        list[index1] = other;
        list[index2] = temp;

        return list;
    }

    public void Select(bool value)
    {
        Img.color = value ? Color.green : Color.white;
    }
}
