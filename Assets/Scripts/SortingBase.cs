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
            
            SelectedItem.Select();
            SelectedItem.PlaySound();
        }
    }

    protected void SelectItem(ChartItem item, bool needCancelOther = false)
    {
        SelectedItem = item;

        if (needCancelOther)
        {
            foreach (var chartItem in ItemList)
            {
                if (chartItem != SelectedItem && chartItem.isSorted == false)
                {
                    chartItem.CancelSelect();
                }
            }
        }

        MainCanvas.Instance.SetPointerPosition(item.pointerPosition.position);
    }

    protected void CancelAllSelect()
    {
        foreach (var item in ItemList)
        {
            if (item.isSorted)
            {
                continue;
            }
            
            item.CancelSelect();
        }
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
    
    protected WaitForSeconds AddWait(float sec)
    {
        return new WaitForSeconds(sec);
    }

    public void StopSorting()
    {
        StopAllCoroutines();
    }
}
