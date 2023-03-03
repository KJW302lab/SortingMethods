using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertSort : SortingBase
{
    private int _index;
    
    public override void Initialize(List<ChartItem> itemList, float speedRate)
    {
        base.Initialize(itemList, speedRate);

        _index = 1;

        StartCoroutine(nameof(StartSort));
    }

    IEnumerator StartSort()
    {
        while (_index < ItemList.Count)
        {
            yield return null;

            var key = ItemList[_index];
            var prev = ItemList[_index - 1];
            SelectItem(key);
            yield return AddWait(0.3f);
            SelectItem(prev);
            yield return AddStep();

            for (int i = ItemList.IndexOf(key); i >= 1; i--)
            {
                if (ItemList[i].Number < ItemList[i - 1].Number)
                {
                    ItemList[i].Switch(ItemList[i - 1], 1f / SpeedRate, ItemList);
                    yield return AddStep();
                }
            }

            CancelAllSelect();

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
