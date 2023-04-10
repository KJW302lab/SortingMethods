using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSort : SortingBase
{
    private float _selectSec;
    private ChartItem _minItem;
    private int _index;

    public override void Initialize(List<ChartItem> itemList, float speedRate)
    {
        base.Initialize(itemList, speedRate);

        _index = 0;
        StartCoroutine(nameof(StartSelectSort));
    }

    IEnumerator StartSelectSort()
    {
        while (_index < ItemList.Count)
        {
            yield return null;

            _minItem = ItemList[_index];

            for (int i = _index; i < ItemList.Count; i++)
            {
                yield return SelectItem(ItemList[i], true, true);

                if (SelectedItem.Number < _minItem.Number)
                {
                    _minItem = SelectedItem;
                }
            }
            
            SelectItem(_minItem, false, true);

            if (ItemList[_index] != _minItem)
            {
                SwapItem(_minItem, ItemList[_index]);
                
                _minItem.OnRightPosition();
            }

            _index++;
        }

        OnSortComplete();
    }
}
