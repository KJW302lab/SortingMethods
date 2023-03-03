using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSort : SortingBase
{
    private int _count;

    public override void Initialize(List<ChartItem> itemList, float speedRate)
    {
        base.Initialize(itemList, speedRate);

        _count = 1;

        StartCoroutine(nameof(StartSort));
    }

    IEnumerator StartSort()
    {
        while (_count < ItemList.Count)
        {
            yield return null;

            for (int i = 0; i < ItemList.Count - _count; i++)
            {
                var first = ItemList[i];
                var second = ItemList[i + 1];
                
                SelectItem(first);
                SelectItem(second);
                yield return AddStep();
                
                if (first.Number > second.Number)
                {
                    first.Switch(second, 1f / SpeedRate, ItemList);
                    yield return AddStep();
                }
                
                CancelAllSelect();
            }
            
            ItemList[^_count].OnRightPosition();

            _count++;
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
