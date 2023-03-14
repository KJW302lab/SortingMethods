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

        StartCoroutine(nameof(StartBubbleSort));
    }

    IEnumerator StartBubbleSort()
    {
        while (_count < ItemList.Count)
        {
            yield return null;

            for (int i = 0; i < ItemList.Count - _count; i++)
            {
                var first = ItemList[i];
                var second = ItemList[i + 1];
                
                yield return SelectItem(first);
                yield return SelectItem(second);
                
                if (first.Number > second.Number)
                {
                    yield return SwapItem(first, second);
                }
                
                CancelAllSelect();
            }

            _count++;
        }

        OnSortComplete();
    }
}
