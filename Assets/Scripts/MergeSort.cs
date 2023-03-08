using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergeSort : SortingBase
{
    private int _index;
    private Dictionary<int, List<ChartItem>> _itemDict = new();
    private List<List<int>> _listToShowNumbers = new();


    public override void Initialize(List<ChartItem> itemList, float speedRate)
    {
        base.Initialize(itemList, speedRate);

        _index = 1;
        _itemDict.Clear();
        _listToShowNumbers.Clear();
        
        Divide(ItemList);

        foreach (var pair in _itemDict)
        {
            List<int> numbers = new();

            foreach (var item in pair.Value)
            {
                numbers.Add(item.Number);
            }
            
            _listToShowNumbers.Add(numbers);
        }

        StartCoroutine(nameof(Show));
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

        return Merge(Divide(leftList), Divide(rightList));
    }

    List<ChartItem> Merge(List<ChartItem> left, List<ChartItem> right)
    {
        List<ChartItem> result = new List<ChartItem>();
        List<ChartItem> listToAdd = new List<ChartItem>();

        while (left.Count > 0 || right.Count > 0)
        {
            if (left.Count > 0 && right.Count > 0)
            {
                if (left[0].Number <= right[0].Number) 
                {
                    result.Add(left[0]);
                    listToAdd.Add(left[0]);
                    left.RemoveAt(0);
                }
                
                else
                {
                    result.Add(right[0]);
                    listToAdd.Add(right[0]);
                    right.RemoveAt(0);
                }
            }
            
            else if (left.Count > 0) 
            {
                result.Add(left[0]);
                listToAdd.Add(left[0]);
                left.RemoveAt(0);
            }
            
            else if (right.Count > 0) 
            {
                result.Add(right[0]);
                listToAdd.Add(right[0]);
                right.RemoveAt(0);
            }
        }
        
        AddToDict(listToAdd, _index);

        _index++;

        return result;
    }

    void AddToDict(List<ChartItem> list, int index)
    {
        _itemDict.Add(index, list);
    }

    ChartItem GetItem(int number)
    {
        foreach (var item in ItemList)
        {
            if (item.Number == number)
            {
                return item;
            }
        }

        return null;
    }

    IEnumerator Show()
    {
        foreach (var numbers in _listToShowNumbers)
        {
            List<ChartItem> itemsToSort = new();

            numbers.Sort();

            foreach (var number in numbers)
            {
                var item = GetItem(number);
                
                SelectItem(item);
                yield return AddStep(1 / SpeedRate);
                
                itemsToSort.Add(item);
            }

            foreach (var item in itemsToSort)
            {
                item.FadeOut(1 / SpeedRate);
                yield return AddWait(1 / SpeedRate);
            }
            
            var sorted = itemsToSort.OrderBy(item => item.Number).ToList();

            List<Vector2> positions = new();

            foreach (var item in sorted)
            {
                positions.Add(item.Rect.anchoredPosition);
            }

            var orderByX = positions.OrderBy(position => position.x).ToList();

            for (var i = 0; i < sorted.Count; i++)
            {
                sorted[i].Rect.anchoredPosition = orderByX[i];
                sorted[i].FadeIn(1 / SpeedRate);
                yield return AddWait(1 / SpeedRate);
            }

            CancelAllSelect();
        }

        var list = ItemList.OrderBy(item => item.Number).ToList();

        foreach (var item in list)
        {
            item.OnRightPosition();
            item.PlaySound();
            item.PointItem();
            yield return AddWait(0.1f);
        }
    }
}
