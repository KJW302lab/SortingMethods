using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeSort : SortingBase
{
    private int[] _originArray;

    public override void Initialize(List<ChartItem> itemList, float speedRate)
    {
        base.Initialize(itemList, speedRate);

        // _originArray = new [ItemList.Count];

        var array = ItemList.ToArray();

        foreach (var item in array)
        {
            // _originArray.
        }

        for (var i = 0; i < ItemList.Count; i++)
        {
            _originArray[i] = ItemList[i].Number;
        }

        Sort(_originArray);

        foreach (var number in _originArray)
        {
            print(number);
        }
        
    }
    
    public void Sort(int[] array)
    {
        Sort(array, 0, array.Length - 1);
    }

    private void Sort(int[] array, int start, int end)
    {
        if (start >= end)
        {
            return;
        }

        int mid = (start + end) / 2;
        Sort(array, start, mid);
        Sort(array, mid + 1, end);
        Merge(array, start, mid, end);
    }

    void Merge(int[] array, int start, int mid, int end)
    {
        int[] temp = new int[end - start + 1];
        int i = start;
        int j = mid + 1;
        int k = 0;

        while (i <= mid && j <= end)
        {
            if (array[i] <= array[j])
            {
                temp[k] = array[i];
                i++;
            }
            else
            {
                temp[k] = array[j];
                j++;
            }

            k++;
        }

        while (i <= mid)
        {
            temp[k] = array[i];
            i++;
            k++;
        }

        while (j <= end)
        {
            temp[k] = array[j];
            j++;
            k++;
        }

        for (k = 0; k < temp.Length; k++)
        {
            array[start + k] = temp[k];
        }
    }
}
