using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeapSort : SortingBase
{
    public override void Initialize(List<ChartItem> itemList, float speedRate)
    {
        base.Initialize(itemList, speedRate);

        StartCoroutine(StartHeapSort());
    }

    IEnumerator StartHeapSort()
    {
        for (int i = ItemList.Count / 2 - 1; i >= 0; i--)
        {
            yield return StartCoroutine(Heapify(ItemList.Count, i));
        }
        
        for (int i = ItemList.Count - 1; i >= 0; i--)
        {
            SelectItem(ItemList[i]);
            yield return SelectItem(ItemList[0]);

            yield return SwapItem(ItemList[i], ItemList[0]);
            
            yield return StartCoroutine(Heapify(i, 0));
        }

        OnSortComplete();
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
            yield return SelectItem(ItemList[largest]);
            yield return SwapItem(ItemList[idx], ItemList[largest]);
            yield return StartCoroutine(Heapify(count, largest));
        }
    }
}
