using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChartItem : MonoBehaviour
{
    public RectTransform pointerPosition;
    public bool isSorted;
    
    public int Number { get; private set; }
    
    private CanvasGroup _canvasGroup;

    public CanvasGroup CanvasGroup
    {
        get
        {
            if (_canvasGroup == null)
            {
                _canvasGroup = GetComponent<CanvasGroup>();
            }

            return _canvasGroup;
        }
    }

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

    private AudioSource _audio;

    private AudioSource Audio
    {
        get
        {
            if (_audio == null)
            {
                _audio = GetComponent<AudioSource>();
            }

            return _audio;
        }
    }

    [SerializeField] private TMP_Text txtNum;

    private int _range;
    
    public void Initialize(float height, float width, int number, int index, int range)
    {
        Rect.anchorMin = Rect.anchorMax = Rect.pivot = Vector2.zero;
        
        Rect.sizeDelta = new Vector2(width * 0.9f, height);
        Rect.anchoredPosition = new Vector2(width * index, 0);

        txtNum.text = number.ToString();

        Number = number;
        _range = range;
    }

    public List<ChartItem> Switch(ChartItem other, float duration, List<ChartItem> list)
    {
        if (other == this)
        {
            return list;
        }
        
        Rect.DOAnchorPos(other.Rect.anchoredPosition, duration)
            .OnStart(() => other.Rect.DOAnchorPos(Rect.anchoredPosition, duration));

        var index1 = list.IndexOf(this);
        var index2 = list.IndexOf(other);

        var temp = this;
        list[index1] = other;
        list[index2] = temp;

        return list;
    }

    public void PlaySound()
    {
        float intervalSize = (4f) / _range; // 구간의 크기 계산

        Audio.pitch = 1 + (intervalSize * Number);
        Audio.PlayOneShot(Audio.clip);
    }

    public void Select()
    {
        Img.color = Color.red;
    }

    public void CancelSelect()
    {
        Img.color = Color.white;
    }

    public void OnRightPosition()
    {
        Img.color = Color.green;
        isSorted = true;
    }

    public void PointItem()
    {
        MainCanvas.Instance.SetPointerPosition(pointerPosition.position);
    }

    public void FadeOut(float duration)
    {
        CanvasGroup.DOFade(0, duration).From(1);
    }

    public void FadeIn(float duration)
    {
        CanvasGroup.DOFade(1, duration).From(0);
    }
}
