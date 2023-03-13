using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RadixSort : SortingBase
{
    private Dictionary<int, Queue<ChartItem>> _bucket;
    private List<ChartItem> _sorted;
    private int _maxNum;
    private int _round = 1;
    private int _slotIndex;
    private float _selectSec;
    private float _switchSec;

    public override void Initialize(List<ChartItem> itemList, float speedRate)
    {
        base.Initialize(itemList, speedRate);

        _bucket = new();

        for (int i = 0; i < 10; i++)
        {
            _bucket[i] = new();
        }

        _sorted = new();

        _sorted = ItemList.OrderBy(item => item.Number).ToList();
        _maxNum = _sorted[^1].Number;

        _round = 1;
        _slotIndex = 0;

        _selectSec = 1 / SpeedRate;
        _switchSec = 1 / SpeedRate;

        StartCoroutine(StartSort());
    }

    IEnumerator StartSort()
    {
        while (_round <= _maxNum)
        {
            yield return null;
            
            foreach (var item in ItemList)
            {
                SelectItem(item);
                yield return AddStep(_selectSec);
                
                int number = item.Number;

                var radix = (number / _round) % 10;
                
                _bucket[radix].Enqueue(item);
                
                item.CancelSelect();
            }

            for (int i = 0; i < _bucket.Count; i++)
            {
                while (_bucket[i].Count > 0)
                {
                    var item = _bucket[i].Dequeue();
                
                    item.Switch(ItemList[_slotIndex], _switchSec, ItemList);
                    _slotIndex++;

                    yield return AddStep(_switchSec);   
                }
            }
            
            _slotIndex = 0;
            _round *= 10;
        }

        foreach (var item in ItemList)
        {
            item.OnRightPosition();
            item.PlaySound();
            item.PointItem();
            yield return AddWait(_selectSec);
        }
    }
}
