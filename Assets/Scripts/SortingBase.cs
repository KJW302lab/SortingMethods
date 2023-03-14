using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    protected float ProcessSec;

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

    protected WaitForSeconds SelectItem(ChartItem item, bool addStep = true, bool needCancelOther = false)
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

        if (addStep)
        {
            Steps++;
        }

        return new WaitForSeconds(ProcessSec);
    }

    protected WaitForSeconds SwapItem(ChartItem origin, ChartItem other)
    {
        if (origin == other)
        {
            return null;
        }

        int originIdx = ItemList.IndexOf(origin);
        int otherIdx = ItemList.IndexOf(other);

        origin.Rect.DOAnchorPos(other.Rect.anchoredPosition, ProcessSec)
            .OnStart(() =>
            {
                other.Rect.DOAnchorPos(origin.Rect.anchoredPosition, ProcessSec);
                
                ItemList.RemoveAt(originIdx);
                ItemList.Insert(originIdx, other);
                ItemList.RemoveAt(otherIdx);
                ItemList.Insert(otherIdx, origin);
            });

        return new WaitForSeconds(ProcessSec);
    }

    protected List<ChartItem> SwapItem(ChartItem origin, ChartItem other, List<ChartItem> list)
    {
        if (origin == other)
        {
            return null;
        }

        int originIdx = list.IndexOf(origin);
        int otherIdx = list.IndexOf(other);

        origin.Rect.DOAnchorPos(other.Rect.anchoredPosition, ProcessSec)
            .OnStart(() =>
            {
                other.Rect.DOAnchorPos(origin.Rect.anchoredPosition, ProcessSec);
            });
        
        var temp = list[originIdx];
        list[originIdx] = other;
        list[otherIdx] = temp;

        return list;
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

    protected void OnSortComplete()
    {
        StartCoroutine(OnRightPosition());
    }

    IEnumerator OnRightPosition()
    {
        foreach (var item in ItemList)
        {
            item.OnRightPosition();
            item.PlaySound();
            item.PointItem();
            yield return AddWait(ProcessSec);
        }
    }

    public virtual void Initialize(List<ChartItem> itemList, float speedRate)
    {
        ItemList = itemList;
        SpeedRate = speedRate;
        ProcessSec = 1 / SpeedRate;
        Steps = 0;
    }
    
    protected WaitForSeconds AddWait(float sec)
    {
        return new WaitForSeconds(sec);
    }

    protected WaitForSeconds AddStep(float sec)
    {
        Steps++;
        return new WaitForSeconds(sec);
    }

    public void StopSorting()
    {
        StopAllCoroutines();
    }
}
