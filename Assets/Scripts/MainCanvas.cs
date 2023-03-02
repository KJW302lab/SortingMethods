using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    [Header("Chart")]
    [SerializeField] private ChartItem chartItem;
    [SerializeField] private RectTransform chartRect;

    [Header("UI")] 
    [SerializeField] private TMP_Text txtSliderValue;
    [SerializeField] private Slider rangeSlider;

    private List<ChartItem> _chartItemList = new();

    private void Awake()
    {
        // ForDebug
        // SetRandomNums(150);
    }

    ChartItem LoadItem()
    {
        var go = Instantiate(chartItem.gameObject, chartRect, false);

        return go.GetComponent<ChartItem>();
    }
    
    public void SetRandomNums(int range)
    {
        var randNumList = RandomNumberGenerator.GenerateRandomNums(range);

        var chartHeight = chartRect.rect.height;
        var chartWidth = chartRect.rect.width;

        if (_chartItemList.Count > 0)
        {
            foreach (var item in _chartItemList)
            {
                Destroy(item);
            }
            
            _chartItemList.Clear();
        }
        
        for (var i = 0; i < randNumList.Count; i++)
        {
            var number = randNumList[i];
            var nodeHeight = chartHeight / range * number;
            var nodeWidth = chartWidth / range;

            var item = LoadItem();
            item.Initialize(nodeHeight, nodeWidth, number, i);
        }
    }

    public void ExecuteSort(SortingMethod method)
    {
        switch (method)
        {
            case SortingMethod.Selection:
                break;
        }
    }

    #region UIControl

    public void SetRangeValue()
    {
        txtSliderValue.text = rangeSlider.value.ToString(CultureInfo.InvariantCulture);
    }

    #endregion
}
