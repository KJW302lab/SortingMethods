using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainCanvas : MonoBehaviour
{
    [Header("Chart")]
    [SerializeField] private ChartItem chartItem;
    [SerializeField] private RectTransform chartRect;

    [Header("UI")] 
    [SerializeField] private TMP_Text txtSliderValue;
    [SerializeField] private TMP_Text txtSortingSteps;
    [SerializeField] private Slider rangeSlider;
    [SerializeField] private TMP_Dropdown methodSelectDrop;
    [SerializeField] private TMP_Dropdown speedRateDrop;
    [SerializeField] private Button btnLaunch;
    [SerializeField] private Button btnSet;
    [SerializeField] private Image pointer;
    [SerializeField] private List<Sprite> pointerSprites = new();

    [Header("SortMethods")]
    [SerializeField] private List<SortingBase> methods = new();

    private List<ChartItem> _chartItemList = new();
    private SortingMethod _method = SortingMethod.Selection;
    private List<float> _speedRates = new() { 1f, 5f, 15f, 40f, 80f, 150f };
    private float _speedRate = 1f;
    private GameObject _selectedModule;
    private SortingBase _selectedMethod;

    private static MainCanvas _instance;

    public static MainCanvas Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<MainCanvas>();

                if (_instance == null)
                {
                    _instance = new GameObject { name = "MainCanvas" }.AddComponent<MainCanvas>();
                }
            }

            return _instance;
        }
    }

    private void Start()
    {
        SetMethod();
        SetSpeedRate();
        btnSet.onClick.AddListener(()=> SetRandomNums((int)rangeSlider.value));
        // btnSet.onClick.AddListener(SetSpecificList);
        btnLaunch.onClick.AddListener(ExecuteSort);
        SetPointerActive(false);

        rangeSlider.value = 50;
    }

    ChartItem LoadItem()
    {
        var go = Instantiate(chartItem.gameObject, chartRect, false);

        return go.GetComponent<ChartItem>();
    }

    void SetSpecificList()
    {
        if (_selectedMethod != null)
        {
            _selectedMethod.StopSorting();
        }
        
        if (_chartItemList.Count > 0)
        {
            foreach (var item in _chartItemList)
            {
                Destroy(item.gameObject);
            }
            
            _chartItemList.Clear();
        }

        List<int> list = new List<int>(){9, 3, 8, 5, 1, 7, 6, 4, 2};
        
        var chartHeight = chartRect.rect.height;
        var chartWidth = chartRect.rect.width;

        for (var i = 0; i < list.Count; i++)
        {
            var number = list[i];
            var nodeHeight = chartHeight / list.Count * number;
            var nodeWidth = chartWidth / list.Count;

            var item = LoadItem();
            item.Initialize(nodeHeight, nodeWidth, number, i, (int)rangeSlider.value);
            
            _chartItemList.Add(item);
        }
    }
    
    void SetRandomNums(int range)
    {
        if (_selectedMethod != null)
        {
            _selectedMethod.StopSorting();
        }
        
        var randNumList = RandomNumberGenerator.GenerateRandomNums(range);

        var chartHeight = chartRect.rect.height;
        var chartWidth = chartRect.rect.width;

        if (_chartItemList.Count > 0)
        {
            foreach (var item in _chartItemList)
            {
                Destroy(item.gameObject);
            }
            
            _chartItemList.Clear();
        }
        
        for (var i = 0; i < randNumList.Count; i++)
        {
            var number = randNumList[i];
            var nodeHeight = chartHeight / range * number;
            var nodeWidth = chartWidth / range;

            var item = LoadItem();
            item.Initialize(nodeHeight, nodeWidth, number, i, (int)rangeSlider.value);
            
            _chartItemList.Add(item);
        }
    }

    SortingBase LoadSortModule(SortingBase module)
    {
        if (_selectedModule != null)
        {
            Destroy(_selectedModule);

            _selectedMethod = null;
        }
        
        var go = Instantiate(module.gameObject, transform);
        _selectedModule = go;

        return go.GetComponent<SortingBase>();
    }

    void ExecuteSort()
    {
        _selectedMethod = LoadSortModule(methods[(int)_method]);
        
        _selectedMethod.Initialize(_chartItemList, _speedRate);
    }

    #region UIControl

    public void SetRangeValue()
    {
        txtSliderValue.text = rangeSlider.value.ToString(CultureInfo.InvariantCulture);
    }

    public void SetPointerPosition(Vector3 position)
    {
        SetPointerActive(true);
        pointer.transform.position = position;
    }

    public void SetPointerSprite(bool value)
    {
        int index = value ? 1 : 2;

        pointer.sprite = pointerSprites[index];
    }

    public void SetSteps(int step)
    {
        txtSortingSteps.text = $"Step : {step.ToString()}";
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
        speedRateDrop.value = 3;
        _speedRate = _speedRates[speedRateDrop.value];
        speedRateDrop.onValueChanged.AddListener((value)=> _speedRate = _speedRates[speedRateDrop.value]);
    }

    void SetPointerActive(bool value)
    {
        pointer.enabled = value;
    }

    #endregion
}
