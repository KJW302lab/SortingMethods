using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
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
    [SerializeField] private TMP_Dropdown methodSelectDrop;
    [SerializeField] private TMP_Dropdown speedRateDrop;
    [SerializeField] private Button btnLaunch;
    [SerializeField] private Button btnSet;

    private List<ChartItem> _chartItemList = new();
    private SortingMethod _method = SortingMethod.Selection;
    private List<float> _speedRates = new() { 1f, 1.5f, 5f, 10f };
    private float _speedRate = 1f;

    private void Start()
    {
        SetMethod();
        SetSpeedRate();
        btnSet.onClick.AddListener(()=> SetRandomNums((int)rangeSlider.value));
        btnLaunch.onClick.AddListener(ExecuteSort);
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

    public void ExecuteSort()
    {
        switch (_method)
        {
            case SortingMethod.Selection:
                break;
            case SortingMethod.Bubble:
                break;
            case SortingMethod.Insert:
                break;
        }
    }

    #region UIControl

    public void SetRangeValue()
    {
        txtSliderValue.text = rangeSlider.value.ToString(CultureInfo.InvariantCulture);
    }

    void SetMethod()
    {
        var methods = Enum.GetNames(typeof(SortingMethod));

        methodSelectDrop.AddOptions(methods.ToList());
        methodSelectDrop.value = 0;
        methodSelectDrop.onValueChanged.AddListener((value)=>
        {
            var methodStr = methodSelectDrop.options[methodSelectDrop.value].text;

            _method = Enum.Parse<SortingMethod>(methodStr);
        });
    }

    void SetSpeedRate()
    {
        List<string> speeds = new();
        
        _speedRates.ForEach((rate)=> speeds.Add($"x{rate.ToString()}"));
        
        speedRateDrop.AddOptions(speeds);
        speedRateDrop.value = 0;
        speedRateDrop.onValueChanged.AddListener((value)=>OnSpeedDropValueChange(_speedRates[speedRateDrop.value]));
    }

    void OnSpeedDropValueChange(float rate)
    {
        _speedRate = rate;
        
        print(_speedRate);
    }

    #endregion
}
