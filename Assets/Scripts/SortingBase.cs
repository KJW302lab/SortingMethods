using System;
using System.Collections.Generic;
using UnityEngine;

public class SortingBase : MonoBehaviour
{
    private int _steps;

    private int Steps
    {
        get => _steps;

        set
        {
            _steps = value;
            
            MainCanvas.Instance.SetSteps(value);
        }
    }
    
    protected List<ChartItem> ItemList;
    protected float SpeedRate;

    private ChartItem _selectedItem;
    protected ChartItem SelectedItem
    {
        get => _selectedItem;
        set
        {
            _selectedItem = value;

            foreach (var item in ItemList)
            {
                item.Select(item == value);
            }
        }
    }

    protected void SelectItem(ChartItem item)
    {
        SelectedItem = item;
        
        item.PlaySound();
        
        MainCanvas.Instance.SetPointerPosition(item.pointerPosition.position);
    }

    public virtual void Initialize(List<ChartItem> itemList, float speedRate)
    {
        ItemList = itemList;
        SpeedRate = speedRate;
        Steps = 0;
    }
    
    protected WaitForSeconds AddStep()
    {
        Steps++;
        return new WaitForSeconds(1 / SpeedRate);
    }
    
    protected WaitForSeconds AddWait()
    {
        return new WaitForSeconds(0.5f / SpeedRate);
    }

    public void StopSorting()
    {
        StopAllCoroutines();
    }
}
