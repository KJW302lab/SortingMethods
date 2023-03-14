using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MergeSort : SortingBase
{
    private int _index;                                         // UI 조작을 위한 변수 (정렬에 관여 X)
    private Dictionary<int, List<ChartItem>> _itemDict = new(); // UI 조작을 위한 변수 (정렬에 관여 X) 
    private List<List<int>> _listToShowNumbers = new();         // UI 조작을 위한 변수 (정렬에 관여 X)


    public override void Initialize(List<ChartItem> itemList, float speedRate)
    {
        base.Initialize(itemList, speedRate);

        // 변수 초기화
        _index = 1;
        _itemDict.Clear();
        _listToShowNumbers.Clear();
        
        MergeSorting(ItemList);

        // UI 조작을 위한 부분 (정렬에 관여 X)
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

    List<ChartItem> MergeSorting(List<ChartItem> itemList)
    {
        // 리스트의 크기가 1이하면 해당 리스트를 그대로 리턴
        if (itemList == null || itemList.Count <= 1)
        {
            return itemList;
        }

        // 리스트의 가운데 요소를 pivot(기준)으로 정함
        int mid = itemList.Count / 2;

        // pivot을 기준으로 하여 2개의 리스트를 새로 생성해 나눔 (Divide)
        List<ChartItem> leftList = new();
        List<ChartItem> rightList = new();

        // 리스트의 첫번째 요소부터 pivot까지 left에 추가
        for (int i = 0; i < mid; i++)
        {
            leftList.Add(itemList[i]);
        }

        // pivot부터 리스트의 마지막 요소까지 right에 추가
        for (int i = mid; i < itemList.Count; i++)
        {
            rightList.Add(itemList[i]);
        }
        
        // 각 리스트의 요소가 1개 이하가 될때까지 나누고, 각 리스트들을 병합함 (Conquer)
        return Merge(MergeSorting(leftList), MergeSorting(rightList));
    }

    List<ChartItem> Merge(List<ChartItem> left, List<ChartItem> right)
    {
        // 결과로 return할 리스트
        List<ChartItem> result = new List<ChartItem>();
        
        // UI 조작을 위한 리스트 (정렬에 관여 X)
        List<ChartItem> listToAdd = new List<ChartItem>();

        
        // 2개의 리스트를 순환하며 result에 병합함
        while (left.Count > 0 || right.Count > 0)
        {
            // 2개의 리스트 모두 요소가 들어있을 경우,
            if (left.Count > 0 && right.Count > 0)
            {
                // 각 리스트의 첫번째 요소의 값을 비교하여 더 작은 것을 result에 추가함
                // 더 작은 것을 result에 추가하면서 오름차순 정렬이 된다.
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
            
            // 2개의 리스트 중 하나만 요소가 남아있을 때,
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

                yield return SelectItem(item);
                
                itemsToSort.Add(item);
            }

            foreach (var item in itemsToSort)
            {
                yield return item.FadeOut(ProcessSec);
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
                yield return sorted[i].FadeIn(ProcessSec);
            }

            CancelAllSelect();
        }

        var list = ItemList.OrderBy(item => item.Number).ToList();

        foreach (var item in list)
        {
            item.OnRightPosition();
            item.PlaySound();
            item.PointItem();
            yield return AddWait(0.1f / SpeedRate);
        }
    }
}
