using System;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class QuickSort : SortingBase
{
    private float _switchSec = 1f;
    private float _selectSec = 1f;
        
    public override void Initialize(List<ChartItem> itemList, float speedRate)
    {
        base.Initialize(itemList, speedRate);

        _switchSec /= speedRate;
        _selectSec /= speedRate;

        StartCoroutine(StartQuickSort(ItemList));
    }

    IEnumerator StartQuickSort(List<ChartItem> list)
    {
        var pivot = list.Count / 2;
        
        SelectItem(list[pivot]);
        yield return AddStep(0.5f);

        int low = 0;
        int high = list.Count - 1;

        while (low < high)
        {
            print($"low {low} high {high} pivot {pivot}");
            
            yield return null;
            
            SelectItem(list[low]);
            SelectItem(list[high]);
            
            if (list[low].Number < list[pivot].Number && low < pivot - 1)
            {
                list[low].CancelSelect();
                low++;
                SelectItem(list[low]);
                yield return AddStep(_selectSec);
            }
            else if (list[high].Number > list[pivot].Number && high > pivot + 1)
            {
                list[high].CancelSelect();
                high--;
                SelectItem(list[high]);
                yield return AddStep(_selectSec);
            }
            else
            {
                if (low < high)
                {
                    list[low].Switch(list[high], _switchSec, ItemList);
                    
                    list[low].CancelSelect();
                    low++;
                    SelectItem(list[low]);

                    list[high].CancelSelect();
                    high--;
                    SelectItem(list[high]);
                    
                    yield return AddStep(_switchSec);
                }
                else
                {
                    
                }
            }
        }

    }
    
}
