using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeapSort : SortingBase
{
    private float _selectSec;
    private float _switchSec;
    private ChartItem[] _heap;
    private int _index;
    
    public override void Initialize(List<ChartItem> itemList, float speedRate)
    {
        base.Initialize(itemList, speedRate);

        _heap = ItemList.ToArray();
        _index = ItemList.Count / 2 - 1;
        _selectSec = 1 / SpeedRate;
        _switchSec = 1 / SpeedRate;

        StartCoroutine(StartSort());
    }

    IEnumerator StartSort()
    {
        while (_index > 0)
        {
            yield return null;
            
            var leaf = ItemList[_index];
            var root = ItemList[0];
            
            SelectItem(leaf);
            SelectItem(root);

            leaf.Switch(root, _switchSec, ItemList);

            yield return AddStep(_switchSec);

            yield return StartCoroutine(Heapify(leaf));

            _index--;
        }
    }

    IEnumerator Heapify(ChartItem parent)
    {
        var largest = ItemList.IndexOf(parent);

        var leftIndex = largest * 2 + 1;
        var rightIndex = leftIndex + 1;

        ChartItem left = null;
        ChartItem right = null;

        if (leftIndex < ItemList.Count)
        {
            left = ItemList[leftIndex];
        }
        
        if (leftIndex < ItemList.Count)
        {
            right = ItemList[rightIndex];
        }

        if (left != null && left.Number > ItemList[largest].Number)
        {
            largest = leftIndex;
        }
        
        if (right != null && right.Number > ItemList[largest].Number)
        {
            largest = rightIndex;
        }

        if (ItemList.IndexOf(parent) != largest)
        {
            parent.Switch(ItemList[largest], _switchSec, ItemList);
            yield return AddStep(_switchSec);
            yield return StartCoroutine(Heapify(ItemList[largest]));
        }
    }
}
