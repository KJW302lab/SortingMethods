using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSort : SortingBase
{
    private ChartItem _minItem;
    private int _index;

    public override void Initialize(List<ChartItem> itemList, float speedRate)
    {
        base.Initialize(itemList, speedRate);

        _index = 0;
        StartCoroutine(nameof(StartSort));
    }

    IEnumerator StartSort()
    {
        while (_index < ItemList.Count)
        {
            yield return null;

            _minItem = ItemList[_index];

            for (int i = _index; i < ItemList.Count; i++)
            {
                SelectItem(ItemList[i], true);
                yield return AddStep();

                if (SelectedItem.Number < _minItem.Number)
                {
                    _minItem = SelectedItem;
                }
            }
            
            SelectItem(_minItem, true);
            yield return AddWait(1);

            if (ItemList[_index] != _minItem)
            {
                ItemList = _minItem.Switch(ItemList[_index], 0.5f, ItemList);
                yield return AddStep();
                yield return AddWait(0.5f);
            }
            
            _minItem.OnRightPosition();

            _index++;
        }

        foreach (var item in ItemList)
        {
            item.OnRightPosition();
            item.PlaySound();
            item.PointItem();
            yield return AddWait(0.1f);
        }
    }
}
