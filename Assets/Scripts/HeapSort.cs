using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeapSort : SortingBase
{
    private float _selectSec;
    private float _switchSec;
    private int _max;
    
    public override void Initialize(List<ChartItem> itemList, float speedRate)
    {
        base.Initialize(itemList, speedRate);
            
        _selectSec = 1 / SpeedRate;
        _switchSec = 1 / SpeedRate;

        _max = ItemList.OrderBy(item => item.Number).ToList()[^1].Number;

        StartCoroutine(StartSort());
    }

    IEnumerator StartSort()
    {
        for (int i = ItemList.Count / 2 - 1; i >= 0; i--)
        {
            yield return StartCoroutine(Heapify(ItemList.Count, i));
        }
        
        for (int i = ItemList.Count - 1; i >= 0; i--)
        {
            SelectItem(ItemList[i]);
            SelectItem(ItemList[0]);
            
            ItemList[i].Switch(ItemList[0], _switchSec, ItemList);
            yield return AddStep(_switchSec);
            yield return StartCoroutine(Heapify(i, 0));
        }

        foreach (var item in ItemList)
        {
            item.OnRightPosition();
            item.PlaySound();
            item.PointItem();
            yield return AddWait(0.1f / SpeedRate);
        }
    }

    IEnumerator Heapify(int count, int idx)
    {
        CancelAllSelect();
        
        var largest = idx;

        var leftIndex = idx * 2 + 1;
        var rightIndex = idx * 2 + 2;

        if (leftIndex < count && ItemList[leftIndex].Number > ItemList[largest].Number)
        {
            largest = leftIndex;
        }
        
        if (rightIndex < count && ItemList[rightIndex].Number > ItemList[largest].Number)
        {
            
            largest = rightIndex;
        }

        if (idx != largest)
        {
            SelectItem(ItemList[idx]);
            SelectItem(ItemList[largest]);
            
            ItemList[idx].Switch(ItemList[largest], _switchSec, ItemList);
            yield return AddStep(_switchSec);
            yield return StartCoroutine(Heapify(count, largest));
        }
    }
}
