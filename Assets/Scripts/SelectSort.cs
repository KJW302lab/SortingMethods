using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSort : SortingBase
{
    public override void Initialize(List<ChartItem> itemList)
    {
        base.Initialize(itemList);
        
        StartSort();
    }

    void StartSort()
    {
        var minItem = ItemList[0];
        
        for (var i = 1; i < ItemList.Count; i++)
        {
            if (ItemList[i].Number < minItem.Number)
            {
                minItem = ItemList[i];
            }
        }
    }
}
