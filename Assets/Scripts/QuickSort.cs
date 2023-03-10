using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuickSort : SortingBase
{
    private float _switchSec = 1f;
    private float _selectSec = 1f;
    private bool _jobFinished;
    private List<ChartItem> _sorted;

    public override void Initialize(List<ChartItem> itemList, float speedRate)
    {
        base.Initialize(itemList, speedRate);

        _switchSec /= speedRate;
        _selectSec /= speedRate;
        _jobFinished = false;

        _sorted = new();

        _sorted = ItemList.OrderBy(item => item.Number).ToList();

        StartCoroutine(StartSorting());
    }

    IEnumerator StartSorting()
    {
        StartCoroutine(QuickSorting(ItemList));

        yield return new WaitUntil(() => ItemList.Count <= 1);

        foreach (var item in _sorted)
        {
            item.OnRightPosition();
            item.PlaySound();
            item.PointItem();
            yield return AddWait(0.1f);
        }
    }

    IEnumerator QuickSorting(List<ChartItem> list)
    {
        if (list.Count <= 1)
        {
            yield break;
        }
        
        int pivot = 0;

        int stored = pivot + 1;

        SelectItem(list[pivot]);
        yield return AddStep(_selectSec);
        
        for (int i = pivot + 1; i < list.Count; i++)
        {
            SelectItem(list[i]);
            yield return AddStep(_selectSec);
            
            if (list[i].Number < list[pivot].Number)
            {
                list[i].Switch(list[stored], _switchSec, ItemList);

                stored++;
                
                yield return AddStep(_switchSec);
            }
            
            CancelAllSelect();
            SelectItem(list[pivot]);
        }

        list[pivot].Switch(list[stored - 1], _switchSec, ItemList);
        yield return AddStep(_switchSec);
        
        list[stored - 1].OnRightPosition();
        list.Remove(list[stored - 1]);

        StartCoroutine(QuickSorting(list));
    }
}
