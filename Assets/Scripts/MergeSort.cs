using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeSort : SortingBase
{
    private List<ChartItem> _sortedList = new();
    private List<List<ChartItem>> _listToShow = new();


    public override void Initialize(List<ChartItem> itemList, float speedRate)
    {
        base.Initialize(itemList, speedRate);
        
        Divide(ItemList);

        foreach (var item in _sortedList)
        {
            print(item.Number);
        }

        StartCoroutine(Show(_listToShow));
    }

    List<ChartItem> Divide(List<ChartItem> itemList)
    {
        if (itemList == null || itemList.Count <= 1)
        {
            return itemList;
        }

        int mid = itemList.Count / 2;

        List<ChartItem> leftList = new();
        List<ChartItem> rightList = new();

        for (int i = 0; i < mid; i++)
        {
            leftList.Add(itemList[i]);
        }

        for (int i = mid; i < itemList.Count; i++)
        {
            rightList.Add(itemList[i]);
        }

        leftList = Divide(leftList);
        rightList = Divide(rightList);

        return Merge(leftList, rightList);
    }

    List<ChartItem> Merge(List<ChartItem> left, List<ChartItem> right)
    {
        List<ChartItem> result = new List<ChartItem>();

        while (left.Count > 0 || right.Count > 0)
        {
            if (left.Count > 0 && right.Count > 0)
            {
                List<ChartItem> items = new();
                
                foreach (var item in left)
                {
                    items.Add(item);
                }
                
                foreach (var item in right)
                {
                    items.Add(item);
                }

                _listToShow.Add(items);
                
                if (left[0].Number <= right[0].Number) 
                {
                    AddToList(left[0]);
                    result.Add(left[0]);
                    left.RemoveAt(0);
                }
                
                else
                {
                    AddToList(right[0]);
                    result.Add(right[0]);
                    right.RemoveAt(0);
                }
            }
            
            else if (left.Count > 0) 
            {
                AddToList(left[0]);
                result.Add(left[0]);
                left.RemoveAt(0);
            }
            
            else if (right.Count > 0) 
            {
                AddToList(right[0]);
                result.Add(right[0]);
                right.RemoveAt(0);
            }
        }

        return result;
    }

    void AddToList(ChartItem item)
    {
        _sortedList.Add(item);
    }

    IEnumerator Show(List<List<ChartItem>> showList)
    {
        yield return null;
        
        // while (true)
        // {
        //     if ()
        //     {
        //         
        //     }
        // }
    }
}
