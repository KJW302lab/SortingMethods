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

        StartCoroutine(nameof(StartInsertSort));
    }

    IEnumerator StartInsertSort()
    {
        while (_index < ItemList.Count)
        {
            yield return null;

            var key = ItemList[_index];
            var prev = ItemList[_index - 1];
            
            yield return SelectItem(key);
            yield return SelectItem(prev);

            for (int i = ItemList.IndexOf(key); i >= 1; i--)
            {
                if (ItemList[i].Number < ItemList[i - 1].Number)
                {
                    yield return SwapItem(ItemList[i], ItemList[i - 1]);
                }
            }

            CancelAllSelect();

            _index++;
        }
        
        OnSortComplete();
    }
}
